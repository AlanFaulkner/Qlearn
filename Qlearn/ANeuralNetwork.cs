using System;
using System.Collections.Generic;

namespace @this
{
    // This class describes and implements a simple feed forward neural network.
    // Information about each neuron contained within the network is stored in a separate Neuron class

    internal class ANeuralNetwork
    {
        // Properties
        private List<List<Neuron>> Network = new List<List<Neuron>> { }; // This is the actual network.

        private List<int> Network_Information = new List<int>() { };     // Each element relates to the number of neurons in a given layer.

        private string ActivationFunction = "";            // Which Activation function is to be used. Current available functions: Sigmoid, TanH,
        private bool RestrictedOuputs = true;              // If false the Activation function only applies to hidden layers. => uses identity function for output layer
        private int Seed = System.Environment.TickCount;   // Seed for random number generation - Used for setting initial weights.

        // Get Parameters
        public string SetActivationFunction
        {
            get { return ActivationFunction; }
            set { ActivationFunction = value; }
        }

        public bool UseActivationFunctionForOutputLayer
        {
            get { return RestrictedOuputs; }
            set { RestrictedOuputs = value; }
        }

        public int SetRNGSeed
        {
            get { return Seed; }
            set { Seed = value; }
        }

        // ##########################
        // ##  Functions - Public  ##
        // ##########################

        // ** Basic I/O Functions **

        // Create new network
        public void Create_Network(List<int> Network_Description)
        {
            Network_Information = Network_Description;

            // Validation of input data
            if (Network_Description.Count < 2)
            {
                Console.Write("The description of neural net you have entered is not valid!\n\nA valid description must contain at least two values:\n   The number of inputs into the network\n   The number of output neurons\n\n In both cases the minimum value allowed is 1!\n\n");
                return;
            }

            for (int i = 0; i < Network_Description.Count; i++)
            {
                if (Network_Description[i] == 0)
                {
                    Console.Write("The description of neural net you have entered is not valid!\n\n The minimum allowed number of neurons or inputs is 1\n\n");
                    return;
                }
            }

            // Construct network

            Random Rnd = new Random(Seed);

            for (int i = 1; i < Network_Description.Count; i++)
            {
                List<Neuron> Layer = new List<Neuron>() { };
                for (int j = 0; j < Network_Description[i]; j++)
                {
                    if (i == 1)
                    {
                        Neuron neuron = new Neuron(Network_Description[0], Rnd.Next());
                        Layer.Add(neuron);
                    }
                    else
                    {
                        Neuron neuron = new Neuron(Network_Description[i - 1], Rnd.Next());
                        Layer.Add(neuron);
                    }
                }
                Network.Add(Layer);
            }
            Console.Write("Network created successfully!\n\n");
            return;
        }

        // Load existing network
        public void Load_Network(string filename)
        {
            // Delete any existing network information
            Network_Information.Clear();
            Network.Clear();

            string line;
            System.IO.StreamReader fs = new System.IO.StreamReader(@"../" + filename);
            while ((line = fs.ReadLine()) != null && line != "Data")
            {
                // convert string to int
                int x;
                Int32.TryParse(line, out x);
                Network_Information.Add(x);
            }

            // Check loaded data validity
            if (Network_Information.Count < 2)
            {
                Console.Write("The description of neural net you have entered is not valid!\n\nA valid description must contain at least two values:\n   The number of inputs into the network\n   The number of output neurons\n\n In both cases the minimum value allowed is 1!\n\n");
                return;
            }

            for (int i = 0; i < Network_Information.Count; i++)
            {
                if (Network_Information[i] < 1)
                {
                    Console.Write("The description of neural net you have entered is not valid!\n\nA valid description must contain at least two values:\n   The number of inputs into the network\n   The number of output neurons\n\n In both cases the minimum value allowed is 1!\n\n");
                    return;
                }
            }

            // Build network based on input data
            for (int i = 1; i < Network_Information.Count; i++)
            {
                List<Neuron> Layer = new List<Neuron>() { };
                for (int j = 0; j < Network_Information[i]; j++)
                {
                    if (i == 1)
                    {
                        Neuron neuron = new Neuron(Network_Information[0], 0);
                        for (int z = 0; z < neuron.Weights.Count; z++)
                        {
                            double x;
                            double.TryParse((line = fs.ReadLine()), out x);
                            neuron.Weights[z] = x;
                        }
                        Layer.Add(neuron);
                    }
                    else
                    {
                        Neuron neuron = new Neuron(Network_Information[i - 1], 0);
                        for (int z = 0; z < neuron.Weights.Count; z++)
                        {
                            double x;
                            double.TryParse((line = fs.ReadLine()), out x);
                            neuron.Weights[z] = x;
                        }
                        Layer.Add(neuron);
                    }
                }
                Network.Add(Layer);
            }

            fs.Close();
        }

        // Save network data
        public void Save_Network(string filename)
        {
            using (System.IO.StreamWriter Out = new System.IO.StreamWriter("../" + filename, false))
            {
                for (int i = 0; i < Network_Information.Count; i++)
                {
                    Out.Write(Network_Information[i] + Environment.NewLine);
                }
                Out.Write("Data" + Environment.NewLine);
                for (int i = 0; i < Network.Count; i++)
                {
                    for (int j = 0; j < Network[i].Count; j++)
                    {
                        for (int k = 0; k < Network[i][j].Weights.Count; k++)
                        {
                            Out.Write(Network[i][j].Weights[k] + Environment.NewLine);
                        }
                    }
                }
                Out.Flush();
                Out.Close();
            }
        }

        // ** Network Information - Used for diagnostic purposes and visualizing neuron weights **

        // Print information about a single neuron identified by the layer (X) and its number (Y) within that layer
        private void Print_Neuron_Info(int X, int Y)
        {
            Console.Write("\n\nNeuron ID: Network[" + X + "][" + Y + "]\n------------------------\n\n");
            Console.Write("Inputs        :  ");
            for (int i = 0; i < Network[X][Y].Inputs.Count; i++) { Console.Write(Network[X][Y].Inputs[i].ToString("F5") + "  "); }
            Console.Write("\n");
            Console.Write("\nWeights + bias:  ");
            for (int i = 0; i < Network[X][Y].Weights.Count; i++) { Console.Write(Network[X][Y].Weights[i].ToString("F5") + "  "); }
            Console.Write("\n");
            Console.Write("\nOutput        :  " + Network[X][Y].Output + Environment.NewLine);
            Console.Write("\nError         :  " + Network[X][Y].Error + Environment.NewLine);
            Console.Write("\nWeight update :  ");
            for (int i = 0; i < Network[X][Y].Weight_Update.Count; i++) { Console.Write(Network[X][Y].Weight_Update[i].ToString("F5") + "  "); }
            Console.Write("\n");
        }

        // Print information for the whole network
        public void Print_Network_Info()
        {
            Console.Write("\n\n#######################\n# Network Information #\n#######################\n\n");
            for (int i = 0; i < Network.Count; i++)
            {
                for (int j = 0; j < Network[i].Count; j++)
                {
                    Print_Neuron_Info(i, j);
                }
            }
        }

        // Print network output for a give input
        public void Print_Network_Output(List<double> Data)
        {
            Calculate_Netowrk_Output(Data);
            for (int i = 0; i < Network[Network.Count - 1].Count; i++) { Console.Write(Network[Network.Count - 1][i].Output + Environment.NewLine); }
        }

        // **  Network functions **

        // Calculate the output of the network
        public List<double> Calculate_Netowrk_Output(List<double> Data)
        {
            // version 1.0 only calculates a single data set output
            List<double> Input_Data = new List<double> { };
            List<double> Output = new List<double> { };
            Input_Data.AddRange(Data); // sets values of the new input data list to be the same as they are in data. note using = just makes a pointer
            Input_Data.Add(1); // default input for the bias

            for (int i = 0; i < Network.Count; i++)
            {
                for (int j = 0; j < Network[i].Count; j++)
                {
                    // iterate through input data and multiple by the weights for each neuron and store the sum
                    // apply activation function to sum
                    // set neuron output to result.

                    double sum = 0;
                    for (int k = 0; k < Input_Data.Count; k++)
                    {
                        Network[i][j].Inputs[k] = Input_Data[k];
                        sum += Network[i][j].Weights[k] * Input_Data[k];
                    }

                    if (i < Network.Count - 1) { sum = Activation_Function(sum, false); }    // apply desired activation function when relevant
                    else if (RestrictedOuputs == true) { sum = Activation_Function(sum, false); }

                    Network[i][j].Output = sum; // set the output value for neuron
                }

                // clear current input data and refill it with the outputs from previous layer to generate inputs into next layer.
                Input_Data.Clear();
                for (int j = 0; j < Network[i].Count; j++)
                {
                    Input_Data.Add(Network[i][j].Output);
                }
                if (i < Network.Count - 1) { Input_Data.Add(1); } // Bias input not needed for last layer
            }
            Output.AddRange(Input_Data); // get output of network

            return Output;
        }

        // Stochastic/Online back-prop employing momentum
        public void Back_prop_Stochastic(List<List<double>> Training_Data, List<List<double>> Target_Data, double Training_Rate = 0.1, double Momentum = 0.7, double Target_Error = 1e-10, double Total_Epochs = 1e7)
        {
            //Console.Write("\n\nTraining network via online back propagation.\n\n");

            //Console.Write(" Training starting.....\n\n Training Results\n------------------\n\n Epoch       Error\n");

            int Epoch = 0;
            double RMS_Error = 0.0;
            do
            {
                // Online training - one input is passed through the net and then weights are updated
                // Data in form of rows
                RMS_Error = 0.0;
                for (int q = 0; q < Training_Data.Count; q++)
                {
                    List<double> Element = new List<double> { };
                    List<double> Element_Result = new List<double> { };
                    Element.AddRange(Training_Data[q]);
                    Element_Result.AddRange(Target_Data[q]);

                    Calculate_Netowrk_Output(Element);
                    for (int j = Network.Count - 1; j >= 0; j--)
                    {
                        for (int k = 0; k < Network[j].Count; k++)
                        {
                            if (j == Network.Count - 1)
                            {
                                Network[j][k].Error = (Element_Result[k] - Network[j][k].Output);
                                if (RestrictedOuputs == true) Network[j][k].Error *= Activation_Function(Network[j][k].Output, true);
                                RMS_Error += Network[j][k].Error * Network[j][k].Error * 0.5;
                                for (int a = 0; a < Network[j][k].Weight_Update.Count; a++)
                                {
                                    Network[j][k].Weight_Update[a] = Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate;
                                    Network[j][k].Weights[a] += Network[j][k].Weight_Update[a] + Network[j][k].Weight_Update_Old[a];
                                    Network[j][k].Weight_Update_Old[a] = Network[j][k].Weight_Update[a] * Momentum;
                                }
                            }
                            else
                            {
                                for (int a = 0; a < Network[j + 1].Count; a++)
                                {
                                    Network[j][k].Error += Network[j + 1][a].Error * Network[j + 1][a].Weights[a];
                                }
                                Network[j][k].Error *= Activation_Function(Network[j][k].Output, true);
                                for (int a = 0; a < Network[j][k].Weight_Update.Count; a++)
                                {
                                    Network[j][k].Weight_Update[a] = Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate;
                                    Network[j][k].Weights[a] += Network[j][k].Weight_Update[a] + Network[j][k].Weight_Update_Old[a];
                                    Network[j][k].Weight_Update_Old[a] = Network[j][k].Weight_Update[a] * Momentum;
                                }
                            }
                        }
                    }
                }
                Epoch++;
                if (Epoch % 1000 == 0) { Console.Write(Epoch + ":  " + RMS_Error + Environment.NewLine); }
            } while (Epoch <= Total_Epochs && RMS_Error > Target_Error);

            //Console.Write("\n\nTraining Complete!\n\n");
            Save_Network("Network_Trained.net");
            //Console.Write("Network saved as 'Network_Trained.net'\n\n");
        }

        public void Back_prop_batch(List<List<double>> Training_Data, List<List<double>> Target_Data, double Training_Rate = 0.5, double Momentum = 0.7, double Target_Error = 1e-10, double Total_Epochs = 1e7)
        {
            int Epoch = 0;
            double RMS_Error = 0.0;
            do
            {
                // batch - multiply input are passed through the net and then weights are updated
                // Data in form of rows
                RMS_Error = 0.0;
                for (int q = 0; q < Training_Data.Count; q++)
                {
                    List<double> Element = new List<double> { };
                    List<double> Element_Result = new List<double> { };
                    Element.AddRange(Training_Data[q]);
                    Element_Result.AddRange(Target_Data[q]);

                    Calculate_Netowrk_Output(Element);
                    for (int j = Network.Count - 1; j >= 0; j--)
                    {
                        for (int k = 0; k < Network[j].Count; k++)
                        {
                            if (j == Network.Count - 1)
                            {
                                Network[j][k].Error = (Element_Result[k] - Network[j][k].Output) * Activation_Function(Network[j][k].Output, true);
                                RMS_Error += Network[j][k].Error * Network[j][k].Error * 0.5;
                                for (int a = 0; a < Network[j][k].Weight_Update.Count; a++)
                                {
                                    if (q == 0) { Network[j][k].Weight_Update[a] = Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate; }
                                    else { Network[j][k].Weight_Update[a] += Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate; }
                                }
                            }
                            else
                            {
                                for (int a = 0; a < Network[j + 1].Count; a++)
                                {
                                    Network[j][k].Error += Network[j + 1][a].Error * Network[j + 1][a].Weights[a];
                                }
                                Network[j][k].Error *= Activation_Function(Network[j][k].Output, true);
                                for (int a = 0; a < Network[j][k].Weight_Update.Count; a++)
                                {
                                    if (q == 0) { Network[j][k].Weight_Update[a] = Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate; }
                                    else { Network[j][k].Weight_Update[a] += Network[j][k].Inputs[a] * Network[j][k].Error * Training_Rate; }
                                }
                            }
                        }
                    }
                }

                // now batch has been completed update weights
                for (int i = 0; i < Network.Count; i++)
                {
                    for (int j = 0; j < Network[i].Count; j++)
                    {
                        for (int k = 0; k < Network[i][j].Weights.Count; k++)
                        {
                            //Network[i][j].Weight_Update[k] *= Training_Rate; // not sure if training rate should just multipy the collective weight update or after each pass
                            Network[i][j].Weights[k] += Network[i][j].Weight_Update[k];
                            Network[i][j].Weight_Update_Old[k] = Network[i][j].Weight_Update[k] * Momentum;
                        }
                    }
                }

                Epoch++;
                if (Epoch % 1000 == 0) { Console.Write(Epoch + ":  " + RMS_Error + Environment.NewLine); }
            } while (Epoch <= Total_Epochs && RMS_Error > Target_Error);
            Console.Write("\n\nTraining Complete!\n\n");
            Save_Network("Network_Trained.net");
            Console.Write("Network saved as 'Network_Trained.net'\n\n");
        }

        // Functions - Private

        private double Activation_Function(double X, bool Differential)
        {
            if (string.IsNullOrEmpty(ActivationFunction)) { throw new InvalidOperationException("No Activation Function has been chosen."); }

            switch (ActivationFunction)
            {
                case ("Sigmoid"):
                    // Limits 0 -> 1
                    if (Differential == true) { return X * (1 - X); }
                    else return 1 / (1 + Math.Exp(-X));

                case ("TanH"):
                    // Limits -1 -> 1
                    if (Differential == true) { return 1 - (Math.Tanh(X) * Math.Tanh(X)); }
                    else return Math.Tanh(X);

                // The following have not been tested! They have been included for completeness

                case ("ArcTan"):
                    // Limits -pi/2 -> pi/2
                    if (Differential == true) { return 1 / (X * X + 1); }
                    else return Math.Atan(X);
                    ;
                case ("Softsign"):
                    // Limits -1 -> 1
                    if (Differential == true) { return 1 / ((1 - Math.Abs(X)) * (1 - Math.Abs(X))); }
                    else return X / (1 - Math.Abs(X));

                case ("Sinusoid"):
                    // Limits -1 -> 1
                    if (Differential == true) { return Math.Cos(X); }
                    else return Math.Sin(X);

                case ("Gaussian"):
                    // Limits 0 -> 1
                    if (Differential == true) { return -2 * X * Math.Exp(-1 * X * X); }
                    else return Math.Exp(-1 * X * X);

                default:
                    return 1;
            }
        }
    }
}