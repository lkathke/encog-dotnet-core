﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.ML.Data;
using Encog.Util;

namespace Encog.ML.HMM.Alog
{
    /// <summary>
    /// The Viterbi algorithm is used to find the most likely sequence of hidden
    /// states (called the Viterbi path) that results in a sequence of observed
    /// events. Used for the Markov information sources, and more generally, hidden
    /// Markov models (HMM).
    /// 
    /// Viterbi AJ (April 1967).
    /// "Error bounds for convolutional codes and an asymptotically optimum decoding algorithm"
    /// . IEEE Transactions on Information Theory 13 (2): 260-269.
    /// doi:10.1109/TIT.1967.1054010.
    /// </summary>
    public class ViterbiCalculator
    {
        private double[][] delta;
        private int[][] psy;
        private int[] _stateSequence;
        private double lnProbability;

        public ViterbiCalculator(IMLDataSet oseq, HiddenMarkovModel hmm)
        {
            if (oseq.Count < 1)
            {
                throw new EncogError("Must not have empty sequence");
            }

            this.delta = EngineArray.AllocateDouble2D((int)oseq.Count, hmm.StateCount);
            this.psy = EngineArray.AllocateInt2D((int)oseq.Count, hmm.StateCount);
            this._stateSequence = new int[oseq.Count];

            for (int i = 0; i < hmm.StateCount; i++)
            {
                this.delta[0][i] = -Math.Log(hmm.GetPi(i))
                        - Math.Log(hmm.getStateDistribution(i).Probability(
                                oseq[0]));
                this.psy[0][i] = 0;
            }

            int t = 1;
            for (int index = 1; index < oseq.Count; index++)
            {
                IMLDataPair observation = oseq[index];

                for (int i = 0; i < hmm.StateCount; i++)
                {
                    ComputeStep(hmm, observation, t, i);
                }

                t++;
            }

            this.lnProbability = Double.PositiveInfinity;
            for (int i = 0; i < hmm.StateCount; i++)
            {
                double thisProbability = this.delta[oseq.Count - 1][i];

                if (this.lnProbability > thisProbability)
                {
                    this.lnProbability = thisProbability;
                    _stateSequence[oseq.Count - 1] = i;
                }
            }
            this.lnProbability = -this.lnProbability;

            for (int t2 = (int)(oseq.Count - 2); t2 >= 0; t2--)
            {
                _stateSequence[t2] = this.psy[t2 + 1][_stateSequence[t2 + 1]];
            }
        }

        private void ComputeStep(HiddenMarkovModel hmm, IMLDataPair o,
                int t, int j)
        {
            double minDelta = Double.PositiveInfinity;
            int min_psy = 0;

            for (int i = 0; i < hmm.StateCount; i++)
            {
                double thisDelta = this.delta[t - 1][i]
                        - Math.Log(hmm.getTransitionProbability(i, j));

                if (minDelta > thisDelta)
                {
                    minDelta = thisDelta;
                    min_psy = i;
                }
            }

            this.delta[t][j] = minDelta
                    - Math.Log(hmm.getStateDistribution(j).Probability(o));
            this.psy[t][j] = min_psy;
        }

        public double LnProbability()
        {
            return this.lnProbability;
        }

        public int[] CopyStateSequence()
        {
            return (int[])_stateSequence.Clone();
        }

    }
}
