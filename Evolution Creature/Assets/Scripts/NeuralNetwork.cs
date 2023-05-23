using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NeuralNetwork : MonoBehaviour
{
    static int inputLayer = 2, hiddenLayer = 4, outputLayer = 2;   // Number of different Nodes (Input, Hidden, Output)

    [SerializeField]
    public double[,] desiredOutputs = new double[outputLayer, 1];              // VALUE DESIRED FOR EACH OUTPUT NEURON

    [Range(-1f, 1f)]
    public double learningRate = 0.5f;

    double[,] actualOutputs = new double[outputLayer, 1];              // OUTPUT GENERATED FOR A GIVEN INPUT DURING FEEDFORWARD

    double[,] inputNodes = new double[inputLayer, 1];
    double[,] hiddenNodes = new double[hiddenLayer, 1];                 // Definition of Nodes matrices (Input, Hidden, Output)
    double[,] outputNodes = new double[outputLayer, 1];

    double[,] weightIH = new double[inputLayer, hiddenLayer];           // Definition of Weight matrix [Input - Hidden]
    double[,] weightHO = new double[hiddenLayer, outputLayer];          // Definition of Weight matrix [Hidden - Output]

    double[,] biasHidden = new double[hiddenLayer, 1];            // Definition of Bias matrix [Input - Hidden] ( BIAS : double, when this node will be fully active)
    double[,] biasOutput = new double[outputLayer, 1];            // Definition of Bias matrix [Hidden - Output]

    double error;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "BOT";
        // RANDOMIZE [INPUT-HIDDEN] WEIGHTS
        for(int i = 0; i < inputLayer; i++)
        {
            for(int j= 0; j < hiddenLayer; j++)
            {
                weightIH[i, j] = Random.Range(-0.5f,0.5f);
            }
        }

        // RANDOMIZE [HIDDEN-OUTPUT] WEIGHTS
        for (int i = 0; i < hiddenLayer; i++)
        {
            for (int j = 0; j < outputLayer; j++)
            {
                weightHO[i, j] = Random.Range(-0.5f, 0.5f);
            }
        }

        // RANDOMIZE HIDDEN BIAS
        for(int i =0;i<hiddenLayer; i++)
        {
            biasHidden[i, 0] = Random.Range(-0.5f, 0.5f);
        }

        // RANDOMIZE OUTPUT BIAS
        for(int i = 0; i < outputLayer; i++)
        {
            biasOutput[i, 0] = Random.Range(-0.5f, 0.5f);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Train();   
        }
           
    }


    void Train()
    {

        inputNodes[0, 0] = -1;
        inputNodes[1, 0] = 1;

        desiredOutputs[0, 0] = -1;
        desiredOutputs[1, 0] = 1;
        
        FeedForward();
        BackPropagation();
    }

   

    void FeedForward() // CALCULATE HIDDEN AND OUTPUT NODES WITH DOT PRODUCT 
    {
        Debug.Log("FEEDFORWARD");
        hiddenNodes = Sigmoid(Add(DotProduct(Transpose(weightIH), inputNodes), biasHidden));
        outputNodes = Sigmoid(Add(DotProduct(Transpose(weightHO), hiddenNodes), biasOutput));
        actualOutputs = outputNodes;
        
    }

    void BackPropagation()
    {
        Debug.Log("BACKPROPAGATION");
        for (int i = 0; i < outputLayer; i++)
        {
            double globalError = DerivSigm(outputNodes[i, 0]) * (desiredOutputs[i, 0] - actualOutputs[i,0]);

            for (int j = 0; j < hiddenLayer; j++)
            {
                // Adjust the weight between the jth hidden node and the output node
                double weightChange = learningRate * globalError * hiddenNodes[j, 0];
                weightHO[i, j] += weightChange;

                // Adjust the bias of the output node
                biasOutput[i,0] += learningRate * globalError;
            }
           
        }

        for (int i = 0; i < hiddenLayer; i++)
        {
            double errorH = DerivSigm(hiddenNodes[i, 0]);

            for (int j = 0; j < inputLayer; j++)
            {
                // Calculate the error for the ith hidden node
                errorH *= weightIH[i, j] * DerivSigm(inputNodes[j, 0]);

                // Adjust the weight between the jth input node and the ith hidden node
                double weightChange = learningRate * errorH * inputNodes[j, 0];
                weightIH[i, j] += weightChange;

                // Adjust the bias of the ith hidden node
                biasHidden[i, 0] += learningRate * errorH;
            }
        }


    }

    // FONCTIONS UTILITAIRES

    private double[,] DotProduct(double[,] m1, double[,] m2)  // DOT PRODUCT 
    {
        int rowsA = m1.GetLength(0);
        int colsA = m1.GetLength(1);

        int rowsB = m2.GetLength(0);
        int colsB = m2.GetLength(1);

        double[,] result = new double[rowsA, colsA];
        int rowsRes = result.GetLength(0);
        int colsRes = result.GetLength(1);

        for (int i = 0; i < rowsRes; i++)
        {
            for (int j = 0; j < colsRes; j++)
            {
                double sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += m1[i, k] * m2[k, j];
                }

                result[i, j] = sum;
            }
        }
        return result;
    }

    private double[,] Transpose(double[,] m1)           // TRANSPOSE OVER DIAGONAL
    {
        double[,] temp = new double[m1.GetLength(1), m1.GetLength(0)];
        Debug.Log(m1.GetLength(1));
        Debug.Log(m1.GetLength(0));
        Debug.Log(temp.GetLength(1));
        Debug.Log(temp.GetLength(0));


        for (int i = 0; i < m1.GetLength(0) ; i++)
        {
           for(int j = 0; j < m1.GetLength(1) ; j++)
            {
                temp[j, i] = m1[i, j];
            }        
        }
        return temp;
    }

    private double[,] Add(double[,] m1, double[,] m2)   // ADD TWO MATRICES
    {
        double[,] temp = new double[m1.GetLength(1), m1.GetLength(0)];

        for (int i = 0; i < m1.GetLength(1); i++)
        {
            for (int j = 0; j < m1.GetLength(0); j++)
            {
                temp[i, j] = m1[i, j] + m2[j,i];
            }
        }
        return temp;
    }

    private double Sigmoid(double x)
    {
        return (1.0f / (1.0f + Mathf.Exp((float) - x)));
    }

    private double DerivSigm(double x)
    {
        return x * (1 - x);
    }

    private double[,] Sigmoid(double[,] m)             // NORMALIZE <0 - 1> Activation Function
    {
        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                m[i,j] = Sigmoid(m[i,j]);
            }
        }
        return m;
        
    }



}
