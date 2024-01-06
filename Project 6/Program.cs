using System;
using System.Collections.Generic;
using System.Linq;

class WeightsAndLoadProblem
{
    private static Random random = new Random(); // Random number generator

    static void Main(string[] args)
    {
        // Provided weights of 30 brick groups
        var bricks = new List<double>
        {
            3.287, 37.856, 14.348, 12.265, 54.674, 7.858, 82.594, 20.718,
            37.189, 72.407, 40.745, 48.012, 32.788, 12.917, 32.394, 89.51,
            43.721, 4.681, 75.317, 41.391, 53.623, 56.557, 95.49, 50.968,
            18.41, 52.727, 80.214, 54.678, 92.533, 70.1
        };

        // Solve the problem with 1000 random restarts
        var solution = RandomRestartHillClimbing(bricks, 250);

        // Display the solution
        for (int i = 0; i < solution.Count; i++)
        {
            Console.WriteLine($"Lorry {i + 1}: {String.Join(", ", solution[i])}");
        }
    }

    
    static List<List<double>> RandomRestartHillClimbing(List<double> bricks, int numRestarts)
    {
        var bestSolution = new List<List<double>>(); // initialize to empty solution
        var bestFitness = double.MaxValue; // initialize to worst possible fitness

        for (int i = 0; i < numRestarts; i++)
        {
            var currentSolution = GenerateRandomSolution(bricks); // Create a solution by randomly assigning bricks to lorries
            var currentFitness = EvaluateFitness(currentSolution); // Evaluate the fitness of the current solution

            while (true)
            {
                var neighbor = GetBestNeighbor(currentSolution); // Find the best neighbor of the current solution
                var neighborFitness = EvaluateFitness(neighbor); // Evaluate the fitness of the neighbor

                // If the neighbor is better, move to the neighbor
                if (neighborFitness < currentFitness)
                {
                    currentSolution = neighbor;
                    currentFitness = neighborFitness;
                }
                // Otherwise, we have reached a local minimum
                else
                {
                    break;
                }
            }
            // If the current solution is better than the best solution, move to the current solution
            if (currentFitness < bestFitness)
            {
                bestSolution = currentSolution;
                bestFitness = currentFitness;
            }
        }
        // print the best fitness
        Console.WriteLine($"Best fitness: {bestFitness}");
        return bestSolution; // Return the best solution
    }

    // Generate a random solution by randomly assigning bricks to lorries
    static List<List<double>> GenerateRandomSolution(List<double> bricks)
    {
        var solution = new List<List<double>> { new List<double>(), new List<double>(), new List<double>() }; // initialize three empty lorries

        // Randomly assign each brick to a lorry
        foreach (var brick in bricks)
        {
            var randomLorry = random.Next(3);
            solution[randomLorry].Add(brick);
        }

        return solution;
    }

    // Evaluate the fitness of a solution
    static double EvaluateFitness(List<List<double>> solution)
    {
        var weights = solution.Select(lorry => lorry.Sum()).ToList(); // Get the weight of each lorry
        return weights.Max() - weights.Min(); // Return the difference between the heaviest and lightest lorry
    }

    // Find the best neighbor of a solution
    static List<List<double>> GetBestNeighbor(List<List<double>> solution)
    {
        var bestNeighbor = new List<List<double>>(); // initialize to empty solution
        var bestFitness = double.MaxValue; // initialize to worst possible fitness
 
        for (int i = 0; i < solution.Count; i++)
        {
            foreach (var brick in solution[i])
            {
                for (int j = 0; j < solution.Count; j++)
                {
                    // Lorry i should be different from lorry j
                    if (i != j)
                    {
                        var newSolution = solution.Select(lorry => new List<double>(lorry)).ToList(); // Copy the current solution
                        newSolution[i].Remove(brick); // Remove the brick from lorry i
                        newSolution[j].Add(brick); // Add the brick to lorry j

                        var newFitness = EvaluateFitness(newSolution); // Evaluate the fitness of the new solution
                        // If the new solution is better than the best neighbor, move to the new solution
                        if (newFitness < bestFitness)
                        {
                            bestNeighbor = newSolution;
                            bestFitness = newFitness;
                        }
                    }
                }
            }
        }

        return bestNeighbor.Count > 0 ? bestNeighbor : solution; // Return the best neighbor if it exists, otherwise return the current solution
    }
}
