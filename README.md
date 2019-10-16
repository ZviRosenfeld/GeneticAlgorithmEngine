# GeneticAlgorithmEngine

GeneticAlgorithmEngine provides an engine for running a Genetic Algorithm that can be easily configured to fit most search domains.

## Download

You can find the GeneticAlgorithmEngine library on nuget.org via package name GeneticAlgorithm.

## Table of Content

- [Usage](#usage)
  - [IChromosome](#ichromosome)
  - [ICrossoverManager](#icrossovermanager)
  - [IPopulationGenerator](#ipopulationgenerator)
    
- [Events](#events)

- [Search Options](#search-options)
  - [Mutations](#mutations)
  - [CancellationToken](#cancellationtoken)
  - [Include All History](#includeallhistory)
  - [Elitism](#elitism)
  - [StopManagers](#istopmanagers)
  - [PopulationRenwalManagers](#ipopulationrenwalmanagers)
  - [MutationProbabilityManager](#imutationprobabilitymanager)
  - [PopulationConverters](#ipopulationconverter)
  - [SelectionStrategies](#iselectionstrategy)
  
- [Ready-Made Components](#ready-made-components)
  - [Chromosomes](#chromosomes)
  - [MutationManagers](#mutationmanagers)
  - [CrossoverManagers](#crossovermanagers)
  - [PopulationGenerators](#populationgenerators)
  - [Example of Using Components](#example-of-using-components)
  
- [Using An Environment](#using-an-environment)
  - [IEnvironment](#ienvironment)
  - [ChromosomeEvaluator](#ichromosomeevaluator)
  
## Usage

GeneticAlgorithmEngine contains 3 interfaces that you'll need to implement.

### IChromosome

Your chromosomes will need to implement the [IChromosome](/GeneticAlgorithm/Interfaces/IChromosome.cs) interface.

```CSharp
    public interface IChromosome
    {
        /// <summary>
        /// Must return a value that is greater then zero
        /// </summary>
        double Evaluate();

        void Mutate();
    }
```

You can find a sample Chromosome [here](/GeneticAlgorithm/Components/Chromosomes/VectorChromosome.cs).

### ICrossoverManager

You'll need to implement the [ICrossoverManager](/GeneticAlgorithm/Interfaces/ICrossoverManager.cs) interface. This tells the engine how to perform crossovers for your chromosomes.
You can read more about crossovers [here](https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)).

```CSharp
    public interface ICrossoverManager
    {
        IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2);
    }
```

You can find some sample CrossoverManagers [here](/GeneticAlgorithm/Components/CrossoverManagers).

### IPopulationGenerator

You'll also need to implement the [IPopulationGenerator](/GeneticAlgorithm/Interfaces/IPopulationGenerator.cs) interface. The engine uses this class to create its initial population. 
The PopulationGenerator will also renew the population when needed (see [IPopulationRenwalManagers](#ipopulationrenwalmanagers)).

```CSharp
    public interface IPopulationGenerator
    {
        /// <summary>
        /// size is the number of chromosomes we want to generate
        /// </summary>
        IEnumerable<IChromosome> GeneratePopulation(int size);
    }
```

You can find some sample PopulationGenerators [here](/GeneticAlgorithm/Components/PopulationGenerators).

## Creating an Instance of GeneticSearchEngine

It's highly recommended that you use the [GeneticSearchEngineBuilder](/GeneticAlgorithm/GeneticSearchEngineBuilder.cs) class to create your GeneticSearchEngine. See the following example.

```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetMutationProbability(0.1).Build();
	
var result = searchEngine.Run();
```

Once you have an instance of an engine you can either use the Run method to run a complete search, or the Next method to run just one more generation.
You can also use the Pause method to pause the search, and then resume it anytime.

```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetMutationProbability(0.1).Build();
	
var result = searchEngine.Next();
Task.Run(() => searchEngine.Run()); // Do in a new thread, so that we don't need to wait for the engine to finish
Thread.Sleep(10); // Give the engine some time to run
searchEngine.Pause();
var task = Task.Run(() => searchEngine.Run());
var result = task.Result;
```

## Events

### OnNewGeneration

This event is called once for every new generations.
This is a good way for GUIs to visually show the argument's progress, or just show the search progress.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator).Build();
searchEngine.OnNewGeneration += (Population population, IEnvironment e) =>
{
	/* Do some work here. For instance:
	IChromosome[] chromosomes = population.GetChromosomes();
	double[] evaluations = population.GetEvaluations();
	*/
};
var result = searchEngine.Run();
```

## Search Options
Let's see how we can configure our search engine to better match our needs.

### Mutations
By default, the probability of mutations is 0. You can change this be using the GeneticSearchEngineBuilder.SetMutationProbability method.
Note that the mutation probability will be ignored if you set a [IMutationProbabilityManager](#imutationprobabilitymanager).

### CancellationToken
You can use the GeneticSearchEngineBuilder.SetCancellationToken method to add cencellationTokens.
The cancellation is checked once per generation, which means that if you're generations take a while to run, there may be a delay between your requesting of the cancellation and the engine actually stopping.

When the cancellation is requested, you'll get the result that was found up till than.

### IncludeAllHistory
If this option is turned on (by default it's off) the result will include the entire history of the population.

### Elitism
Using elitism, you can set a percentage of the best chromosomes that will be passed "as is" to the next generation.
You can read more about elitism [here](https://en.wikipedia.org/wiki/Genetic_algorithm#Elitism).

### IStopManagers
[StopManagers](/GeneticAlgorithm/Interfaces/IStopManager.cs) let you configure when you want the search to stop. 
You can create your own managers by implementing the IStopManager interface, or use one of the existing managers.
Note that there is no limit to the number of StopManagers you can add to your search engine.

You can find a tutorial on creating a custom StopManager [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Creating-a-Custom-StopManager).
In addition, [here](/GeneticAlgorithm/StopManagers) are some examples of custom StopManagers.

Existing StopManagers:
- [StopAtEvaluation](/GeneticAlgorithm/StopManagers/StopAtEvaluation.cs): Will cause the search to stop when it reaches some predefined evaluation.
- [StopAtConvergence](/GeneticAlgorithm/StopManagers/StopAtConvergence.cs): The search will stop when the difference between chromosomes in the population is too small.
- [StopIfNoImprovment](/GeneticAlgorithm/StopManagers/StopIfNoImprovment.cs): Will stop if the improvement over 'X' generations isn't good enough.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddStopManager(new StopAtConvergence(0.5)).Build();

var result = searchEngine.Run();
```

### IPopulationRenwalManagers
[PopulationRenwalManagers](/GeneticAlgorithm/Interfaces/IPopulationRenwalManager.cs) will tell the engine to renew a certain percentage of the population if some condition is met. 
You can create your own managers by implementing the IPopulationRenwalManager interface, or use one of the existing managers.
Note that there is no limit to the number of PopulationRenwalManagers you can add to your search engine.

You can find a tutorial on creating a custom PopulationRenwalManager [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Creating-a-Custom-PopulationRenwalManager).
In addition, [here](/GeneticAlgorithm/PopulationRenwalManagers) are some examples of custom PopulationRenwalManagers.

Existing PopulationRenwalManagers:
- [RenewAtConvergence](/GeneticAlgorithm/PopulationRenwalManagers/RenewAtConvergence.cs): The search will renew some of the population if the difference between chromosomes in the population is too small.
- [RenewIfNoImprovment](/GeneticAlgorithm/PopulationRenwalManagers/RenewIfNoImprovment.cs): Will renew some of the population if the improvement over 'X' generations isn't good enough.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddPopulationRenwalManager(new RenewAtConvergence(0.5, 0.5)).Build();

var result = searchEngine.Run();
```

### IMutationProbabilityManager

The [IMutationProbabilityManager](/GeneticAlgorithm/Interfaces/IMutationProbabilityManager.cs) interface lets you dynamically determine the probability of a mutation based on the current population.
For instance, you might want to set a high mutation probability for a few generations if the population is homogeneous, and lower it while the population is diversified.

You can find an exsample of a custom MutationManager [here](/GeneticAlgorithm/MutationProbabilityManagers/ConvergenceMutationProbabilityManager.cs).

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetCustomMutationProbabilityManager(new MyMutationProbabilityManager()).Build();

var result = searchEngine.Run();
```

### IPopulationConverter

The [IPopulationConverter](/GeneticAlgorithm/Interfaces/IPopulationConverter.cs) interface provides you with a very powerful tool for customizing your search.
The IPopulationConverter method ConvertPopulation is called every generation after the population is created. In this method you can change the population in any way you want.
This allows you to add [Lamarckian evolution](https://amitksaha.wordpress.com/2009/12/04/lamarckism-in-genetic-algorithms/) to your algorithm - that is, let the chromosomes improve themselves before generating the children.

There is no limit to the number of PopulationConverters you can add to your search. If you add more than one, they will be called in the order in which they were added.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddPopulationConverter(new MyPopulationConverter()).Build();

var result = searchEngine.Run();
```

You can find an example of a custom PopulationConverter [here](/GeneticAlgorithm/PopulationConverters/SamplePopulationConverter.cs).

### ISelectionStrategy

The [ISelectionStrategy](/GeneticAlgorithm/Interfaces/ISelectionStrategy.cs) tells the engine how to choose the chromosomes that will create the next generation.
You can create your own SelectionStrategy by implementing the ISelectionStrategy interface, or use one of the existing strategies.
By default, the engine will use the RouletteWheelSelection, but you can changed that with the GeneticSearchEngineBuilder's SetSelectionStrategy method.

Existing SelectionStrategies:
- [RouletteWheelSelection](/GeneticAlgorithm/SelectionStrategies/RouletteWheelSelection.cs): With RouletteWheelSelection, the chance of choosing a chromosome is equal to the chromosome's fitness divided by the total fitness. In other words, if we have two chromosomes, A and B, where A.Evaluation == 6 and B.Evaluation == 4, there's a 60% change of choosing A, and a 40% change of choosing B.
- [TournamentSelection](/GeneticAlgorithm/SelectionStrategies/TournamentSelection.cs): With TournamentSelection, we choose a random n chromosomes from the population, and of them select the chromosome with the highest evaluation. In TournamentSelection, selection pressure will grow as the tournament size grows. See [this](https://en.wikipedia.org/wiki/Tournament_selection) link for more information.
- [StochasticUniversalSampling](/GeneticAlgorithm/SelectionStrategies/StochasticUniversalSampling.cs): StochasticUniversalSampling (SUS) is very similar to RouletteWheelSelection. For more information look [here](https://en.wikipedia.org/wiki/Stochastic_universal_sampling).

You can find examples of ISelectionStrategies [here](/GeneticAlgorithm/SelectionStrategies).

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetSelectionStrategy(new TournamentSelection(5)).Build();

var result = searchEngine.Run();
```

## Ready-Made Components

Components are ready-made implementations of commonly used genetic components (such as CrossoverManagers, MutationManagers and chromosomes).

### Chromosomes

The [VectorChromosome<T>](/GeneticAlgorithm/Components/Chromosomes/VectorChromosome.cs) is an implementation of the IChromosome interface.
It holds a generic vector, which is set in its constructor and can be retrieved via the GetVector method.

VectorChromosome expects an [IMutationManager<T>](#mutationmanagers) and [IEvaluator](/GeneticAlgorithm/Components/Interfaces/IEvaluator.cs) in its constructor, which tell it how to preform mutations and evaluate itself.

```CSharp
    class BasicEvaluator : IEvaluator
    {
        public double Evaluate(IChromosome chromosome) =>
            ((VectorChromosome<int>) chromosome).GetVector().Sum();
    }
	
    class UseVectorChromosome
    {
    	IMutationManager mutationManager = new UniformMutationManager(0, 10);
        IEvaluator evaluator = new BasicEvaluator();
		int[] vector = new int[] {1, 3, 2, 8};
        VectorChromosome<int> = new VectorChromosome<int>(vector, mutationManager, evaluator);
    }
```

### MutationManagers

[IMutationManager<T>](/GeneticAlgorithm/Components/Interfaces/IMutationManager.cs) tells the VectorChromosome<T> how to preform mutations.
You can create your own MutationManager by implementing the IMutationManager<T> interface, or use an existing managers.

Existing managers:
- [BitStringMutationManager](/GeneticAlgorithm/Components/MutationManagers/BitStringMutationManager.cs): This mutation only works on binary chromosomes. It flips bits at random (that is replaces 1 with 0 and 0 with 1). The probability of a bit being flipped is 1 / \<vector-length>\.
- [BoundaryMutationManager](/GeneticAlgorithm/Components/MutationManagers/BoundaryMutationManager.cs): This mutation operator replaces the genome with either lower or upper bound randomly (works only on integer-vector chromosomes). The probability of a bit being replaced is 1 / \<vector-length>\.
- [UniformMutationManager](/GeneticAlgorithm/Components/MutationManagers/UniformMutationManager.cs): This mutation operator replaces the genome with a random value between the lower and upper bound (works only on integer-vector chromosomes). The probability of a bit being replaced is 1 / \<vector-length>\.
- [GaussianMutationManager](/GeneticAlgorithm/Components/MutationManagers/GaussianMutationManager.cs): This operator adds a unit Gaussian distributed random value to the chosen gene (works only on integer-vector chromosomes). If it falls outside of the user-specified lower or upper bounds for that gene, the new gene value is clipped.

### CrossoverManagers

The CrossoverManagers are implementations of the [ICrossoverManager](#icrossovermanager) interface.

- [SinglePointCrossoverManager<T>](/GeneticAlgorithm/Components/CrossoverManagers/SinglePointCrossoverManager.cs): A point on both parents' chromosomes is picked randomly, and designated a 'crossover point'. Bits to the right of that point are swapped between the two parent chromosomes. See [this](https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)#Single-point_crossover) for more information.
- [K_PointCrossoverManager<T>](/GeneticAlgorithm/Components/CrossoverManagers/K_PointCrossoverManager.cs): Similar to SinglePointCrossoverManager, only that K points are chosen instead of one. See [this](https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)#Two-point_and_k-point_crossover) for more information.
- [UniformCrossoverManager<T>](/GeneticAlgorithm/Components/CrossoverManagers/UniformCrossoverManager.cs): In uniform crossover, each bit is chosen from either parent with equal probability.

### PopulationGenerators

PopulationGenerators are implementations of the [IPopulationGenerator](#ipopulationgenerator) interface.

- [IntVectorChromosomePopulationGenerator](/GeneticAlgorithm/Components/PopulationGenerators/IntVectorChromosomePopulationGenerator.cs): Generates a population of VectorChromosome<int> within some min and max bounds. 
- [BinaryVectorChromosomePopulationGenerator](/GeneticAlgorithm/Components/PopulationGenerators/BinaryVectorChromosomePopulationGenerator.cs): Generates binary chromosomes (chromosomes of type VectorChromosome<int> with all ones and zeros).

### Example of Using Components

Following is an example of using components:

```CSharp
    class BasicEvaluator : IEvaluator
    {
        public double Evaluate(IChromosome chromosome) =>
            ((VectorChromosome<int>) chromosome).GetVector().Sum();
    }
	
    class UseComponents
    {
	IMutationManager mutationManager = new UniformMutationManager(0, 10);
        IEvaluator evaluator = new BasicEvaluator();
        IPopulationGenerator populationGenerator =
                new IntVectorChromosomePopulationGenerator(VECTOR_SIZE, 0, 10, mutationManager, evaluator);
        ICrossoverManager crossoverManager = new SinglePointCrossoverManager<int>(mutationManager, evaluator);
        GeneticSearchEngine engine =
            new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATION, crossoverManager, populationGenerator).Build();
        SearchReasult result = engine.Run();
    }
```

## Using an Environment

Sometimes, it's impossible to evaluate a chromosome without knowing information about it's surroundings, such as the rest of the population. (This, by the way, in the case in nature - where the fitness an individual depends on its environment and the way it interacts with the other individuals).

GeneticAlgorithmEngine provides two classes to deal with this.

### IEnvironment

The [IEnvironment](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IEnvironment.cs) represents the "environment". 
You can create your own environment by implementing the IEnvironment interface. If you don't, we will use the [DefaultEnvironment](/GeneticAlgorithm/DefaultEnvironment.cs) class, which contains the other chromosomes, and the generation number.

The environment's UpdateEnvierment is called before the evaluation of a generation begins, which lets you configure your environment based on the current population.
UpdateEnvierment is guaranteed to be called once per generation.

You can find an example of a custom Environment [here](/EnvironmentGui/MyEnvironment.cs).

### IChromosomeEvaluator

If you set the [IChromosomeEvaluator](/GeneticAlgorithm/Interfaces/IChromosomeEvaluator.cs), the engine will use your ChromosomeEvaluator's evaluate method (and not the chromosome's default evaluate method).
Since the IChromosomeEvaluator's SetEnvierment is called before the evaluation begins, your ChromosomeEvaluator can use the information in the environment to evaluate the chromosomes.

You can find an example of a custom ChromosomeEvaluator [here](/EnvironmentGui/ChromosomeEvaluator.cs).

### Example of Using an Environment

```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetEnvironment(new MyEnvironment) // If you don't set an environment, we'll use the DefaultEnvironment class
	.SetCustomChromosomeEvaluator(new MyChromosomeEvaluator()).Build();

var result = searchEngine.Run();
```

### Tutorial

You can find a tutorial on using an environment [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Using-an-Environment).
The tutorial's full source code (alone with a poorly designed GUI) is located [here](/EnvironmentGui).
