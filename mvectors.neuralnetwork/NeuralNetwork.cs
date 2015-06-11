using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace mvectors.neuralnetwork
{
    public class NeuralNetwork
    {
        private Matrix<double> Theta0;
        private Matrix<double> Theta1;

        private Matrix<double> Delta0;
        private Matrix<double> Delta1;

        public NeuralNetwork(int inputNodes, int hiddenNodes)
        {
            Theta0 = Matrix<double>.Build.Random(inputNodes, hiddenNodes);
            Theta1 = Matrix<double>.Build.Random(hiddenNodes, 1);
            Delta0 = Matrix<double>.Build.Dense(inputNodes,hiddenNodes);
            Delta1 = Matrix<double>.Build.Dense(hiddenNodes, 1);

            // (hiddenNodes, inputNodes);
        }

        public void Train1(Vector<double> input, Vector<double> expected)
        {
            var z2 = Theta0.Multiply(input);
            var hidden = z2.Map(SpecialFunctions.Logistic);

            var z3 = Theta1.Multiply(hidden);
            var output = z3.Map(SpecialFunctions.Logistic);

            //calculate errors and gradients etc
            var error = output - expected; //delta4

            var hiddenError = Theta1.Transpose().Multiply(error).PointwiseMultiply(
                output.PointwiseMultiply(1 - output));

            var inputError = Theta0.Transpose().Multiply(hiddenError).PointwiseMultiply(
                hiddenError.PointwiseMultiply(1 - hiddenError));

        }
        public struct Example
        {
            public Vector<double> X;
            public Vector<double> Y;
        }

        private Vector<double> BackPropogate(Matrix<double> thetaL, Vector<double> activationL,
            Vector<double> deltaLPlus1)
        {
            return
                thetaL.TransposeThisAndMultiply(deltaLPlus1)
                    .PointwiseMultiply(activationL.PointwiseMultiply(1 - activationL));
        }
        public void TrainSet(IEnumerable<Example> testData)
        {
            double lambda = 0.0;
            double m = 1.0;
            var a = new Vector<double>[3];
            var d = new Vector<double>[3];
            Delta0.Clear();
            Delta1.Clear();
            foreach (var sample in testData)
            {
                m = (double)sample.X.Count;
                a[0] = sample.X;
                a[1] = Theta0.Multiply(a[0]).Map(SpecialFunctions.Logistic);
                a[2] = Theta1.Multiply(a[1]).Map(SpecialFunctions.Logistic);
                d[2] = a[2] - sample.Y;
                d[1] = BackPropogate(Theta1, a[1], d[2]);
//                d[0] = BackPropogate(Theta0, a[0], d[1]);
                Delta1 = Delta1 + d[2].ToColumnMatrix().Multiply(a[1].ToColumnMatrix());
                Delta0 = Delta0 + d[1].ToColumnMatrix().Multiply(a[0].ToColumnMatrix());

            }
            var D0 = Delta0.Multiply(1/m);// ignoring regularisation + Theta0.Multiply(lambda);
            var D1 = Delta1.Multiply(1/m);
            
            var unrolledTheta0 = (Theta0.RowCount*Theta0.ColumnCount);
            var unrolledTheta1 = (Theta1.RowCount + Theta1.ColumnCount);
            var thetaVec = new double[unrolledTheta0 + unrolledTheta1];
            var DVec = new double[unrolledTheta0 + unrolledTheta1];

            Theta0.ToColumnWiseArray().CopyTo(thetaVec,0);
            Theta1.ToColumnWiseArray().CopyTo(thetaVec, unrolledTheta1);

            D0.ToColumnWiseArray().CopyTo(DVec,0);
            D1.ToColumnWiseArray().CopyTo(DVec,unrolledTheta1);

            //lbfgsb.ComputeMin(BananaFunction, BananaFunctionGradient, initialGuess);
        }

        //public Vector<double> Delta(Vector<double> activation, )

        public double Cost(Vector<double> input, Vector<double> expected, Vector<double> output)
        {
            var m = (double)input.Count;
            var logHTheta = output.Map(SpecialFunctions.Logistic);
            var term1 = expected.PointwiseMultiply(logHTheta);

            var term2 = expected.Map(d => 1 - d).PointwiseMultiply(output.Map(d => SpecialFunctions.Logistic(1 - d)));

            return (-1.0d/m)*(term1 + term2).Sum();

        }
    }
}
