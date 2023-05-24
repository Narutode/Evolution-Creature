using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NNV2 : MonoBehaviour
{
    // Node numbers
    public int ni = 4;
    public int nh = 4;
    public int no = 2;


    // Activation for nodes
    public double[] ai, ah, ao;

    // Weights
    double[,] wi, wo;

    double[,] ci, co;

    double[][][] testPatterns = new double[4][][];

    private void Awake()
    {
        testPatterns = new double[4][][];

        testPatterns[0] = new double[][]
        {
            new double[] { 0, 0, 1, 1 },
            new double[] { 0, 0 }
        };

        testPatterns[1] = new double[][]
        {
            new double[] { 0, 1, 0, 0 },
            new double[] { 1, 0 }
        };

        testPatterns[2] = new double[][]
        {
            new double[] { 1, 0, 1, 0 },
            new double[] { 1, 1 }
        };

        testPatterns[3] = new double[][]
        {
            new double[] { 1, 1, 0, 1 },
            new double[] { 0, 1 }
        };


        //  {
        //      {{a, b}, {c, d}},
        //      {{a, b}, {c, d}}
        //  }

        ni += 1; // +1 for bias node

        // Activation for nodes
        ai = new double[ni];
        ah = new double[nh];
        ao = new double[no];

        for (int i = 0; i < ni; i++)
        {
            ai[i] = 1.0;
        }
        for (int i = 0; i < nh; i++)
        {
            ah[i] = 1.0;
        }
        for (int i = 0; i < no; i++)
        {
            ao[i] = 1.0;
        }

        // Create weights
        wi = new double[ni, nh];
        wo = new double[nh, no];

        // Randomize weights
        for (int i = 0; i < ni; i++)
        {
            for (int j = 0; j < nh; j++)
            {
                wi[i, j] = UnityEngine.Random.Range(-2, 2);
            }
        }

        for (int i = 0; i < nh; i++)
        {
            for (int j = 0; j < no; j++)
            {
                wo[i, j] = UnityEngine.Random.Range(-2, 2);
            }
        }

        // last change in weights for momentum
        ci = new double[ni, nh];
        co = new double[nh, no];
    }

    double[] Updating(double[] inputs)
    {
        // input activations
        for (int i = 0; i < ni - 1; i++)
        {
            //ai[i] = Sigmoid(inputs[i]);
            ai[i] = inputs[i];
        }

        // hidden activations
        for (int i = 0; i < nh; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < ni; j++)
            {
                sum += ai[j] * wi[j, i];
            }
            ah[i] = Sigmoid(sum);
        }

        // output activations
        for (int i = 0; i < no; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < nh; j++)
            {
                sum += ah[j] * wo[j, i];

            }
            ao[i] = Sigmoid(sum);

        }

        double[] res = ao;
        return res;
    }

    double BackPropagate(double[] targets, double N, double M)
    {
        double error;

        // calculate error terms for output
        double[] output_deltas = new double[no];
        for (int i = 0; i < no; i++)
        {
            output_deltas[i] = 0.0;
            error = targets[i] - ao[i];
            output_deltas[i] = DSigmoid(ao[i]) * error;
        }

        // calculate error terms for hidden
        double[] hidden_deltas = new double[nh];
        for (int i = 0; i < nh; i++)
        {
            error = 0.0;
            for (int j = 0; j < no; j++)
            {
                error += output_deltas[j] * wo[i, j];
            }
            hidden_deltas[i] = DSigmoid(ah[i]) * error;
        }

        // update output weights
        for (int i = 0; i < nh; i++)
        {
            for (int j = 0; j < no; j++)
            {
                double change = output_deltas[j] * ah[i];
                wo[i, j] += N * change + M * co[i, j];
                co[i, j] = change;
            }

        }

        // update input weights
        for (int i = 0; i < ni; i++)
        {
            for (int j = 0; j < nh; j++)
            {
                double change = hidden_deltas[j] * ai[i];
                wi[i, j] += N * change + M * ci[i, j];
                ci[i, j] = change;
            }

        }

        // calculate error
        error = 0.0;
        for (int i = 0; i < targets.Length; i++)
        {
            error += 0.5 * ((targets[i] - ao[i]) * (targets[i] - ao[i]));
        }

        return error;

    }


    void Test(double[][][] patterns)
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            for (int j = 0; j < patterns[i].Length; j++)
            {
                Debug.Log(patterns[i][0][j] + "->" + Updating(patterns[i][0])[j]);
            }

        }

    }
    public void Use(double[] inputs)
    {


    }


    void PrintWeights()
    {
        Debug.Log("Input weights:");
        for (int i = 0; i < ni; i++)
        {
            for (int j = 0; j < nh; j++)
            {
                Debug.Log(wi[i, j]);
            }

        }
    }

    void Train(double[][][] patterns, int iterations = 1000, double N = 0.5, double M = 0.1)
    {
        // N : learning rate, M : momentum factor

        for (int i = 0; i < iterations; i++)
        {
            double error = 0.0;
            for (int j = 0; j < patterns.Length; j++)
            {

                double[] inputs = patterns[j][0];
                double[] targets = patterns[j][1];
                Updating(inputs);
                error += BackPropagate(targets, N, M);

            }
            if (i % 100 == 0)
            {
                Debug.Log($"error {error,-14}");
            }
        }
    }

    public void Demo()
    {
        Train(testPatterns);
        Test(testPatterns);
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Demo();
    }

    private double Sigmoid(double x)
    {
        return (1.0f / (1.0f + Mathf.Exp((float)-x)));
    }

    private double DSigmoid(double x)
    {
        return x * (1 - x);
    }

    /*private double[,] Sigmoid(double[,] m)             // NORMALIZE <0 - 1> Activation Function
    {
        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                m[i, j] = Sigmoid(m[i, j]);
            }
        }
        return m;

    }
    */
}
