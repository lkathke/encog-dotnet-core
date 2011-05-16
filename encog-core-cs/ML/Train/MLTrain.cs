using System.Collections.Generic;
using Encog.ML.Data;
using Encog.ML.Train.Strategy;
using Encog.Neural.Networks.Training.Propagation;

namespace Encog.ML.Train
{
    /// <summary>
    /// Defines a training method for a machine learning method.  Most MLMethod 
    /// objects need to be trained in some way before they are ready for use.
    /// </summary>
    ///
    public interface MLTrain
    {
        /// <value>The training implementation type.</value>
        TrainingImplementationType ImplementationType { /// <returns>The training implementation type.</returns>
            get; }


        /// <value>True if training can progress no further.</value>
        bool TrainingDone { /// <returns>True if training can progress no further.</returns>
            get; }


        /// <value>The training data to use.</value>
        MLDataSet Training { /// <returns>The training data to use.</returns>
            get; }


        /// <value></value>
        double Error { /// <summary>
            /// Get the current error percent from the training.
            /// </summary>
            ///
            /// <returns>The current error.</returns>
            get;
            /// <param name="error"></param>
            set; }


        /// <summary>
        /// Set the current training iteration.
        /// </summary>
        ///
        /// <value>Iteration.</value>
        int IterationNumber { /// <returns>The current training iteration.</returns>
            get;
            /// <summary>
            /// Set the current training iteration.
            /// </summary>
            ///
            /// <param name="iteration">Iteration.</param>
            set; }


        /// <returns>True if the training can be paused, and later continued.</returns>
        bool CanContinue { get; }

        /// <summary>
        /// Get the current best machine learning method from the training.
        /// </summary>
        ///
        /// <value>The best machine learningm method.</value>
        MLMethod Method { /// <summary>
            /// Get the current best machine learning method from the training.
            /// </summary>
            ///
            /// <returns>The best machine learningm method.</returns>
            get; }


        /// <value>The strategies to use.</value>
        IList<IStrategy> Strategies { /// <returns>The strategies to use.</returns>
            get; }

        /// <summary>
        /// Perform one iteration of training.
        /// </summary>
        ///
        void Iteration();

        /// <summary>
        /// Should be called once training is complete and no more iterations are
        /// needed. Calling iteration again will simply begin the training again, and
        /// require finishTraining to be called once the new training session is
        /// complete.
        /// It is particularly important to call finishTraining for multithreaded
        /// training techniques.
        /// </summary>
        ///
        void FinishTraining();

        /// <summary>
        /// Perform a number of training iterations.
        /// </summary>
        ///
        /// <param name="count">The number of iterations to perform.</param>
        void Iteration(int count);

        /// <summary>
        /// Pause the training to continue later.
        /// </summary>
        ///
        /// <returns>A training continuation object.</returns>
        TrainingContinuation Pause();

        /// <summary>
        /// Resume training.
        /// </summary>
        ///
        /// <param name="state">The training continuation object to use to continue.</param>
        void Resume(TrainingContinuation state);

        /// <summary>
        /// Training strategies can be added to improve the training results. There
        /// are a number to choose from, and several can be used at once.
        /// </summary>
        ///
        /// <param name="strategy">The strategy to add.</param>
        void AddStrategy(IStrategy strategy);
    }
}