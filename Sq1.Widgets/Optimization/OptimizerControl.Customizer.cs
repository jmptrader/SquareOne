﻿using System;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;
using Sq1.Core;
using Sq1.Core.Indicators;
using Sq1.Core.Optimization;
using Sq1.Core.Repositories;

namespace Sq1.Widgets.Optimization {
	public partial class OptimizerControl {
		Color colorBackgroundRed;
		Color colorBackgroundGreen;
		
		void olvBacktests_FormatRow(object sender, FormatRowEventArgs e) {
			SystemPerformanceRestoreAble systemPerformanceRestoreAble = e.Model as SystemPerformanceRestoreAble;
			if (systemPerformanceRestoreAble == null) return;
			e.Item.BackColor = (systemPerformanceRestoreAble.NetProfitForClosedPositionsBoth > 0.0) ? this.colorBackgroundGreen : this.colorBackgroundRed;
			//if (value == ethalonRedIfLessBlueIfGreater) return this.ForeColor;
			//return (value > ethalonRedIfLessBlueIfGreater) ? Color.Blue : Color.Red;
		}
		void olvBacktestsCustomizeColors() {
			//if (this.snap.Colorify) {
				this.colorBackgroundRed = Color.FromArgb(255, 230, 230);
				this.colorBackgroundGreen = Color.FromArgb(230, 255, 230);
				this.olvBacktests.UseCellFormatEvents = true;
				this.olvBacktests.FormatRow += new EventHandler<FormatRowEventArgs>(olvBacktests_FormatRow);
			//} else {
			//	this.olvPositions.UseCellFormatEvents = false;
			//	this.olvPositions.FormatRow -= new EventHandler<FormatRowEventArgs>(olvPositions_FormatRow);
			//}
		}
		void olvBacktestsCustomize() {
			this.olvBacktestsCustomizeColors();
			this.olvcSerno.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcSerno.AspectGetter: SystemPerformanceRestoreAble=null";
				return (this.backtestsLocalEasierToSync.IndexOf(systemPerformanceRestoreAble) + 1).ToString();
			};
			this.olvcNetProfit.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcNetProfit.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.NetProfitForClosedPositionsBothFormatted;
			};
			this.olvcTotalPositions.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcTotalTrades.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.PositionsCountBothFormatted;
			};
			this.olvcProfitPerPosition.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcAverageProfit.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.AvgProfitBothFormatted;
			};
			this.olvcWinLoss.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcWinLoss.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.WinLossRatioFormatted;
			};
			this.olvcProfitFactor.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcProfitFactor.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.ProfitFactorFormatted;
			};
			this.olvcRecoveryFactor.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcRecoveryFactor.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.RecoveryFactorFormatted;
			};
			this.olvcMaxDrawdown.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcMaxDrawdown.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.MaxDrawDownFormatted;
			};
			this.olvcMaxConsecutiveWinners.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble SystemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (SystemPerformanceRestoreAble == null) return "olvcMaxConsecutiveWinners.AspectGetter: SystemPerformanceRestoreAble=null";
				return SystemPerformanceRestoreAble.MaxConsecWinnersFormatted;
			};
			this.olvcMaxConsecutiveLosers.AspectGetter = delegate(object o) {
				SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				if (systemPerformanceRestoreAble == null) return "olvcMaxConsecutiveLosers.AspectGetter: SystemPerformanceRestoreAble=null";
				return systemPerformanceRestoreAble.MaxConsecLosersFormatted;
			};
			
			//v1
			foreach (OLVColumn colDynParam in this.columnsDynParams) {
			//v2
			//foreach (string fullNameHopefullySortedAsAdded in this.columnsDynByParamName.Keys) {
			//	OLVColumn colDynParam = this.columnsDynByParamName[fullNameHopefullySortedAsAdded];
			//v3 hopefully Values are also sorted as added, 
			//foreach (OLVColumn colDynParam in this.columnsDynByParamName.Values) {
				//v1
				//AspectGetterDelegateWrapper individualDelgateForEachColumn = new AspectGetterDelegateWrapper(colDynParam.Name);
				//colDynParam.AspectGetter = individualDelgateForEachColumn
				//string colDynParamNameStatic = colDynParam.Name;
				//colDynParam.AspectGetter = new AspectGetterDelegate(object o) {
				//	string colDynParamNameStatic2 = colDynParam.Name;
				//	if (colDynParamNameStatic2 != colDynParamNameStatic) {
				//		//Debugger.Break();	// THIS_IS_WHY_I_HATE_LAMBDAS
				//	}

				//	SystemPerformanceRestoreAble SystemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
				//	if (SystemPerformanceRestoreAble == null) return colDynParamNameStatic + ".AspectGetter: SystemPerformanceRestoreAble=null";
				//	if (SystemPerformanceRestoreAble.ScriptAndIndicatorParameterClonesByName.ContainsKey(colDynParamNameStatic) == false) {
				//		return colDynParamNameStatic + ".AspectGetter: !SystemPerformanceRestoreAble.ScriptAndIndicatorParametersByName[" + colDynParamNameStatic + "]";
				//	}
				//	IndicatorParameter param = SystemPerformanceRestoreAble.ScriptAndIndicatorParameterClonesByName[colDynParamNameStatic];
				//	return param.ValueCurrent.ToString();
				//};
				// v2: cool but it didn't help
				AspectGetterDelegateWrapper individualDelgateForEachColumn = new AspectGetterDelegateWrapper(colDynParam.Name);
				colDynParam.AspectGetter = (AspectGetterDelegate) individualDelgateForEachColumn.AspectGetterDelegateImplementor;
			}
		}
		
		
		void olvHistoryCustomize() {
			this.olvHistoryCustomizeColors();
			this.olvcHistoryDate.AspectGetter = delegate(object o) {
				FnameDateSizeColor fnameDateSize = o as FnameDateSizeColor;
				if (fnameDateSize == null) return "olvcHistoryDate.AspectGetter: fnameDateSize=null";
				return fnameDateSize.DateSmart;
			};
			this.olvcHistorySymbolScaleRange.AspectGetter = delegate(object o) {
				FnameDateSizeColor fnameDateSize = o as FnameDateSizeColor;
				if (fnameDateSize == null) return "olvcHistorySymbolScaleRange.AspectGetter: fnameDateSize=null";
				return fnameDateSize.Name;
			};
			this.olvcHistorySize.AspectGetter = delegate(object o) {
				FnameDateSizeColor fnameDateSize = o as FnameDateSizeColor;
				if (fnameDateSize == null) return "olvcHistorySize.AspectGetter: fnameDateSize=null";
				return fnameDateSize.SizeMb;
			};
			this.olvcPFavg.AspectGetter = delegate(object o) {
				FnameDateSizeColor fnameDateSize = o as FnameDateSizeColor;
				if (fnameDateSize == null) return "olvcProfitFactor.AspectGetter: fnameDateSize=null";
				return fnameDateSize.PFavg.ToString();
			};
			this.olvBacktests.ShowItemToolTips = true;
		}
		void olvHistoryCustomizeColors() {
			this.olvHistory.FormatRow += new EventHandler<FormatRowEventArgs>(olvHistory_FormatRow);
		}
		void olvHistory_FormatRow(object sender, FormatRowEventArgs e) {
			FnameDateSizeColor fname = e.Model as FnameDateSizeColor;
			if (fname == null) return;
			if (fname.PFavg == 0) return;	// SystemPerformanceRestoreAble didn't have ProfitFactor calculated/deserialized
			e.Item.BackColor = (fname.PFavg >= 1) ? this.colorBackgroundGreen : this.colorBackgroundRed;
		}

		void olvParametersCustomize() {
			this.olvPrametersCustomizeBold();
			this.olvcParamName.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamName.AspectGetter: param=null";
				return param.FullName;
			};
			this.olvcParamNumberOfRuns.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamNumberOfRuns.AspectGetter: param=null";
				return param.NumberOfRuns;
			};
			this.olvcParamValueMin.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamValueMin.AspectGetter: param=null";
				return param.ValueMin;
			};
			this.olvcParamValueCurrent.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamValueCurrent.AspectGetter: param=null";
				return param.ValueCurrent;
			};
			this.olvcParamValueMax.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamValueMax.AspectGetter: param=null";
				return param.ValueMax;
			};
			this.olvcParamStep.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamStep.AspectGetter: param=null";
				return param.ValueIncrement;
			};
			this.olvcParamWillBeSequenced.AspectGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return "olvcParamWillBeSequenced.AspectGetter: param=null";
				return param.WillBeSequencedDuringOptimization;
			};

			this.olvPrameters.CheckStateGetter = delegate(object o) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return CheckState.Indeterminate;
				return param.WillBeSequencedDuringOptimization ? CheckState.Checked : CheckState.Unchecked;
			};
			this.olvPrameters.CheckStatePutter = delegate(object o, CheckState newState) {
				IndicatorParameter param = o as IndicatorParameter;
				if (param == null) return CheckState.Indeterminate;
				param.WillBeSequencedDuringOptimization = newState.CompareTo(CheckState.Checked) == 0;
				this.olvPrameters.RefreshObject(param);
				this.optimizer.TotalsCalculate();
				this.totalsPropagateAdjustSplitterDistance();
				// for HeaderAllCheckBox.Clicked => Strategy.Serialize()d as many times as you got (Script+Indicator)Parameters
				this.optimizer.Executor.Strategy.Serialize();
				return newState;
			};
		}

		void olvPrametersCustomizeBold() {
			this.olvPrameters.UseCellFormatEvents = true;
			this.olvPrameters.FormatCell += new EventHandler<FormatCellEventArgs>(olvPrameters_FormatCell);
		}
		void olvPrameters_FormatCell(object sender, FormatCellEventArgs e) {
			IndicatorParameter imMouseOver = e.Model as IndicatorParameter;
			if (imMouseOver == null) {
				string msg = "MUST_BE_IndicatorParameter_OR_ScriptParameter";
				//e.CellValue = msg;
				Assembler.PopupException(msg);
				return;
			}
			if (imMouseOver.WillBeSequencedDuringOptimization) {
				if (e.Column == this.olvcParamValueCurrent) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Regular);
				}
				if (e.Column == this.olvcParamValueMin) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Bold);
				}
				if (e.Column == this.olvcParamValueMax) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Bold);
				}
			} else {
				if (e.Column == this.olvcParamValueCurrent) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Bold);
				}
				if (e.Column == this.olvcParamValueMin) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Regular);
				}
				if (e.Column == this.olvcParamValueMax) {
					e.SubItem.Font = new Font(e.Item.Font, FontStyle.Regular);
				}
			}
		}
	}
	class AspectGetterDelegateWrapper {
		string colDynParamName;
		public AspectGetterDelegateWrapper(string colDynParamName) {
			this.colDynParamName = colDynParamName;
		}
		public string AspectGetterDelegateImplementor(object o) {
			SystemPerformanceRestoreAble systemPerformanceRestoreAble = o as SystemPerformanceRestoreAble;
			if (systemPerformanceRestoreAble == null) return colDynParamName + ".AspectGetter: SystemPerformanceRestoreAble=null";
			if (systemPerformanceRestoreAble.ScriptAndIndicatorParameterClonesByName_BuiltOnBacktestFinished == null) {
				return "WASNT_INVOKED:SystemPerformance.BuildStatsOnBacktestFinished()";
			}
			if (systemPerformanceRestoreAble.ScriptAndIndicatorParameterClonesByName_BuiltOnBacktestFinished.ContainsKey(colDynParamName) == false) {
				return colDynParamName + ".AspectGetter: !SystemPerformanceRestoreAble.ScriptAndIndicatorParametersByName[" + colDynParamName + "]";
			}
			IndicatorParameter param = systemPerformanceRestoreAble.ScriptAndIndicatorParameterClonesByName_BuiltOnBacktestFinished[colDynParamName];
			return param.ValueCurrent.ToString();
		}
	}
}
