using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Sq1.Core.Livesim;

namespace Sq1.Core.StrategyBase {
	public partial class Strategy {
		[JsonProperty]	public Guid Guid;
		[JsonProperty]	public string								Name;
		[JsonProperty]	public string								ScriptSourceCode;
		[JsonProperty]	public string								DotNetReferences;
		[JsonProperty]	public string								DllPathIfNoSourceCode;
		[JsonProperty]	public int									ExceptionsLimitToAbortBacktest;

		[JsonProperty]	public string								StoredInJsonAbspath;
		[JsonIgnore]	public string								StoredInFolderRelName	{ get { return Path.GetFileName(Path.GetDirectoryName(this.StoredInJsonAbspath)); } }
		[JsonIgnore]	public string								StoredInJsonRelName		{ get { return Path.GetFileName(this.StoredInJsonAbspath); } }
		[JsonIgnore]	public string								RelPathAndNameForOptimizationResults			{ get { return Path.Combine(this.StoredInFolderRelName, this.Name); } }
		
		[JsonIgnore]	public bool									ActivatedFromDll { get {
				if (string.IsNullOrEmpty(this.DllPathIfNoSourceCode)) return false;
				if (this.DllPathIfNoSourceCode.Length <= 4) return false;
				string substr = this.DllPathIfNoSourceCode.Substring(DllPathIfNoSourceCode.Length - 4);
				return substr.ToUpper() == ".DLL";
			} }
		[JsonProperty]	public bool									HasChartOnly { get { return string.IsNullOrEmpty(this.StoredInFolderRelName); } }
		[JsonIgnore]	public Script								Script;
		//CANT_DESERIALIZE_JsonException public Dictionary<int, ScriptParameter> ScriptParametersByIdJSONcheck { get {	// not for in-program use; for a human reading Strategy's JSON
		//					return this.Script.ParametersById;
		//				} }
		[JsonIgnore]	public string								ScriptParametersAsStringByIdJSONcheck { get {	// not for in-program use; for a human reading Strategy's JSON
				if (this.Script == null) return null;
				return this.Script.ScriptParametersAsString;
			} }
		[JsonIgnore]	public string								IndicatorParametersAsStringByIdJSONcheck { get {	// not for in-program use; for a human reading Strategy's JSON
				if (this.Script == null) return null;
				return this.Script.IndicatorParametersAsString;
			} }
		[JsonProperty]	public string								ScriptContextCurrentName;	// if you restrict SET, serializer won't be able to restore from JSON { get; private set; }
		[JsonProperty]	public Dictionary<string, ContextScript>	ScriptContextsByName;
		[JsonIgnore]	public ContextScript						ScriptContextCurrent { get {
				lock (this.ScriptContextCurrentName) {	// Monitor shouldn't care whether I change the variable that I use for exclusive access...
				//v2 lock (this.scriptContextCurrentNameLock) {
					if (this.ScriptContextsByName.ContainsKey(ScriptContextCurrentName) == false)  {
					string msg = "ScriptContextCurrentName[" + ScriptContextCurrentName + "] doesn't exist in Strategy[" + this.ToString() + "]";
						#if DEBUG
						Debugger.Break();
						#endif
						throw new Exception(msg);
					}
					return this.ScriptContextsByName[this.ScriptContextCurrentName];
				}
			} }
		[JsonIgnore]	public ScriptCompiler						ScriptCompiler;
		// I_DONT_WANT_TO_BRING_CHART_SETTINGS_TO_CORE public ChartSettings ChartSettings;
		[JsonProperty]	public LivesimBrokerSettings				LivesimBrokerSettings;
		[JsonProperty]	public LivesimStreamingSettings				LivesimStreamingSettings;
		//v1 [JsonProperty]	public Dictionary<string, List<SystemPerformanceRestoreAble>>	OptimizationResultsByContextIdent;

	
		// programmer's constructor
		public Strategy(string name) : this() {
			this.Name = name;
		}
		// deserializer's constructor
		public Strategy() {
			this.Guid = Guid.NewGuid();
			this.ScriptContextCurrentName			= ContextScript.DEFAULT_NAME;
			this.ScriptContextsByName				= new Dictionary<string, ContextScript>();
			this.ScriptContextsByName.Add(this.ScriptContextCurrentName, new ContextScript(this.ScriptContextCurrentName));
			this.ScriptCompiler						= new ScriptCompiler();
			this.ExceptionsLimitToAbortBacktest 	= 10;
			this.scriptContextCurrentNameLock		= new object();
			this.LivesimBrokerSettings				= new LivesimBrokerSettings(this);
			this.LivesimStreamingSettings			= new LivesimStreamingSettings(this);
			//v1this.OptimizationResultsByContextIdent	= new Dictionary<string, List<SystemPerformanceRestoreAble>>();
		}
		public override string ToString() {
			string ret = this.Name;
			//v1, infinite recursion if (this.ScriptContextCurrent != null) {
			if (this.ScriptContextsByName.ContainsKey(this.ScriptContextCurrentName) == false) {
				ret += "NOT_FOUND_ScriptContextCurrentName[" + this.ScriptContextCurrentName + "]";
				return ret;
			}
			ret += "[" + this.ScriptContextCurrent.ToString() + "]";
			//ret += this.ScriptParametersAsStringByIdJSONcheck + this.IndicatorParametersAsStringByIdJSONcheck;
			return ret;
		}
		public void CompileInstantiate() {
			if (this.ActivatedFromDll) return;
			this.Script = this.ScriptCompiler.CompileSourceReturnInstance(this.ScriptSourceCode, this.DotNetReferences);
		}

		public Strategy CloneWithNewGuid() {
			Strategy ret = (Strategy)base.MemberwiseClone();
			ret.Guid = Guid.NewGuid();
			return ret;
		}
		//v1: pre-DisposableExecutor
		//public Strategy CloneWithNewScriptInstanceResetContextsToSingle_clonedForOptimizer(ContextScript ctxNext, ScriptExecutor executorCloneForOptimizer) {
		//    Strategy ret = (Strategy)base.MemberwiseClone();
		//    if (this.Script != null) {
		//        //WILL_THROW: public Strategy Strategy { get { return this.Executor.Strategy } }
		//        ret.Script = (Script) Activator.CreateInstance(this.Script.GetType());
		//        //ret.Script = ret.Script.Clo);
		//        //executorCloneForOptimizer.Initialize(executorCloneForOptimizer.ChartShadow, ret);		//sets ret.Script.Strategy = ret;
		//        //ret.Script.Initialize(executorCloneForOptimizer);
		//    } else {
		//        string msg = "I_REFUSE_TO_CLONE_STRATEGY_FOR_OPTIMIZER__SCRIPT_NULL";
		//        Assembler.PopupException(msg);
		//        return null;
		//    }
		//    Dictionary<string, ContextScript> ctxSingle = new Dictionary<string, ContextScript>();
		//    ctxSingle.Add(ctxNext.Name, ctxNext);
		//    ret.ScriptContextsByName = ctxSingle;
		//    ret.ScriptContextCurrentName = ctxNext.Name;

		//    //MOVED_UPSTACK ret.ContextSwitchCurrentToNamedAndSerialize(ctxNext.Name, false);
		//    return ret;
		//}
		//v2 I_NEED_SCRIPT_ONLY__WILL_FULLY_RECONSTRUCT_CONTEXT_FROM_PARAMETERS_SEQUENCER
		internal Strategy CloneMinimalForEachThread_forEachDisposableExecutorInOptimizerPool() {
			Strategy ret = (Strategy)base.MemberwiseClone();

			// I don't want ScriptDerived's own List and Dictionaries to refer to the Strategy.Script running live now;
			this.Script = (Script) Activator.CreateInstance(this.Script.GetType());

			// each ParameterSequencer.Next() (having its own NAME), will be absorbed to DEFAULT; messing with Dictionary to keep synchronized is too costly
			this.ScriptContextsByName = new Dictionary<string, ContextScript>();
			this.ScriptContextsByName.Add(this.ScriptContextCurrentName, new ContextScript(this.ScriptContextCurrentName));
			this.ScriptContextCurrentName = ContextScript.DEFAULT_NAME;
			this.ScriptContextCurrent.CloneResetAllToMinForOptimizer();

			this.ScriptCompiler				= null;
			this.LivesimBrokerSettings		= null;
			this.LivesimStreamingSettings	= null;
			return ret;
		}

		// CHANGING_SLIDERS_ALREADY_AFFECTS_SCRIPT_AND_INDICATOR_PARAMS_KOZ_THERE_ARE_NO_CLONES_ANYMORE
		//public void PushChangedScriptParameterValueToScriptAndSerialize(ScriptParameter scriptParameter) {
		//    //ScriptParameters are only identical objects between script context and sliders.tags, while every click-change is pushed into Script.ParametersByID)
		//    int paramId = scriptParameter.Id;
		//    double valueNew = scriptParameter.ValueCurrent;
		//    if (this.Script.ScriptParametersById_ReflectedCached.ContainsKey(paramId) == false) {
		//        string msg = "YOU_CHANGED_SCRIPT_PARAMETER_WHICH_NO_LONGER_EXISTS_IN_SCRIPT";
		//        Assembler.PopupException(msg);
		//        return;
		//    }

		//    double valueOld = this.Script.ScriptParametersById_ReflectedCached[paramId].ValueCurrent;
		//    if (valueOld == valueNew) {
		//        string msg = "SLIDER_CHANGED_TO_VALUE_SCRIPT_PARAMETER_ALREADY_HAD [" + valueOld + "]=[" + valueNew + "]";
		//        Assembler.PopupException(msg, null, false);
		//        return;
		//    }
		//    this.Script.ScriptParametersById_ReflectedCached[paramId].ValueCurrent = valueNew;
		//}
		//public void PushChangedIndicatorParameterValueToScriptAndSerialize(IndicatorParameter iParamChangedCtx) {
		//    //new concept that IndicatorParameters are only identical objects between script context and sliders.tags, while every click-change is absorbed by snapshot.IndicatorsInstancesReflected
		//    string indicatorName = iParamChangedCtx.IndicatorName;
		//    string indicatorParameterName = iParamChangedCtx.FullName;
		//    Dictionary<string, Indicator> indicatorsByName = this.Script.IndicatorsByName_ReflectedCached;
		//    if (indicatorsByName.ContainsKey(indicatorName) == false) {
		//        string msg = "WILL_PICK_UP_ON_BACKTEST__INDICATOR_NOT_FOUND_IN_INDICATORS_REFLECTED: " + indicatorName;
		//        Assembler.PopupException(msg);
		//        return;
		//    }
		//    Indicator indicatorInstantiated = indicatorsByName[indicatorName];
		//    if (indicatorInstantiated.ParametersByName.ContainsKey(indicatorParameterName) == false) {
		//        string msg = "INDICATOR_PARAMETER_NOT_FOUND_FOR_INDICATOR_REFLECTED_FOUND: " + indicatorParameterName;
		//        Assembler.PopupException(msg);
		//        return;
		//    }
		//    IndicatorParameter iParamInstantiated = indicatorInstantiated.ParametersByName[indicatorParameterName];
		//    double valueNew = iParamChangedCtx.ValueCurrent;
		//    double valueOld = iParamInstantiated.ValueCurrent;
		//    if (valueOld == valueNew) {
		//        string msg = "SLIDER_CHANGED_TO_VALUE_INDICATOR_PARAMETER_ALREADY_HAD [" + valueOld + "]=[" + valueNew + "]";
		//        Assembler.PopupException(msg);
		//        return;
		//    }
		//    iParamInstantiated.AbsorbCurrentFixBoundariesIfChanged(iParamChangedCtx);
		//}

		public void ResetScriptAndIndicatorParametersInCurrentContextToScriptDefaultsAndSave() {
			if (this.Script == null) {
				string msg = "SCRIPT_IS_NULL_CAN_NOT_RESET_PARAMETERS_TO_CLONE_CONSTRUCTED";
				Assembler.PopupException(msg);
				return;
			}
			try {
				this.Script.SwitchToDefaultContextByAbsorbingScriptAndIndicatorParametersFromSelfCloneConstructed();
				//ALREADY_SAVED_BY_LINE_ABOVE this.Serialize();
				string msg = "Successfully reset ScriptContextCurrentName[" + this.ScriptContextCurrentName + "] for strategy[" + this + "]";
				Assembler.DisplayStatus(msg);
			} catch (Exception ex) {
				Assembler.PopupException("ResetScriptAndIndicatorParametersToScriptDefaults()", ex);
			}
		}
		public void Serialize() {
			Assembler.InstanceInitialized.RepositoryDllJsonStrategy.StrategySave(this);
		}

		public int ScriptParametersReflectedAbsorbMergeFromCurrentContext_SaveStrategy(bool saveStrategy = false) {
			int currentValuesAbsorbed = this.ScriptContextCurrent.ScriptParametersReflectedAbsorbFromCurrentContextReplace(
					this.Script.ScriptParametersById_ReflectedCached);
			if (currentValuesAbsorbed > 0 && saveStrategy == true) this.Serialize();
			return currentValuesAbsorbed;
		}

		internal int IndicatorParametersReflectedAbsorbMergeFromCurrenctContext_SaveStrategy(bool saveStrategy = false) {
			int currentValuesAbsorbed = this.ScriptContextCurrent.IndicatorParamsReflectedAbsorbFromCurrentContextReplace(
					this.Script.IndicatorParametersByIndicator_ReflectedCached);
			if (currentValuesAbsorbed > 0 && saveStrategy == true) this.Serialize();
			return currentValuesAbsorbed;
		}
	}
}
