﻿using System.Diagnostics;

namespace NeuralNetworkLibrary.NetworkStructure
{
    [DebuggerDisplay ("Weight: {Weight}")]
    public class Dendrite
    {
        public Neuron Previous { get; }
        public Neuron Next { get; }
        public double Weight { get; set; }

        public double WeightUpdate { get; set; }

        public Dendrite(Neuron previous, Neuron next, double weight)
        {
            Previous = previous;
            Next = next;
            Weight = weight;
        }

        public double Compute() => Previous.ActivatedOutput * Weight;
    }
}