﻿using System;
using System.Diagnostics;

using Sq1.Core.DataTypes;
using Sq1.Core.Execution;
using Sq1.Core.Streaming;
using Sq1.Core.Indicators;

namespace Sq1.Core.Livesim {
	public class LivesimQuoteBarConsumer : IStreamingConsumer {
				Livesimulator livesimulator;
		public	LivesimQuoteBarConsumer(Livesimulator livesimulator) {
			this.livesimulator = livesimulator;
		}
				Bars IStreamingConsumer.ConsumerBarsToAppendInto { get { return livesimulator.BarsSimulating; } }
		void IStreamingConsumer.UpstreamSubscribedToSymbolNotification(Quote quoteFirstAfterStart) {
		}
		void IStreamingConsumer.UpstreamUnSubscribedFromSymbolNotification(Quote quoteLastBeforeStop) {
		}
		void IStreamingConsumer.ConsumeQuoteOfStreamingBar(Quote quote) {
			ReporterPokeUnit pokeUnitNullUnsafe = this.livesimulator.Executor.ExecuteOnNewBarOrNewQuote(quote);
		}
		void IStreamingConsumer.ConsumeBarLastStaticJustFormedWhileStreamingBarWithOneQuoteAlreadyAppended(Bar barLastFormed, Quote quoteForAlertsCreated) {
			string msig = " //BacktestQuoteBarConsumer.ConsumeBarLastStaticJustFormedWhileStreamingBarWithOneQuoteAlreadyAppended";
			if (barLastFormed == null) {
				string msg = "THERE_IS_NO_STATIC_BAR_DURING_FIRST_4_QUOTES_GENERATED__ONLY_STREAMING"
					+ " Backtester starts generating quotes => first StreamingBar is added;"
					+ " for first four Quotes there's no static barsFormed yet!! Isi";
				Assembler.PopupException(msg + msig, null, false);
				return;
			}
			msig += "(" + barLastFormed.ToString() + ")";
			//v1 this.backtester.Executor.Strategy.Script.OnBarStaticLastFormedWhileStreamingBarWithOneQuoteAlreadyAppendedCallback(barLastFormed);
			ReporterPokeUnit pokeUnitNullUnsafe = this.livesimulator.Executor.ExecuteOnNewBarOrNewQuote(quoteForAlertsCreated, false);
		}
		public override string ToString() {
			string ret = "CONSUMER_FOR_" + this.livesimulator.ToString();
			return ret;
		}
	}
}
