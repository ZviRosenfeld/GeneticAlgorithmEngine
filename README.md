# GeneticAlgorithmEngine

GeneticAlgorithmEngine provides an engine for running a Genetic Algorithm that can be easily configured to fit most search domains.

## Download

You can find the GeneticAlgorithmEngine library on nuget.org via package name GeneticAlgorithm.

## Usage

GeneticAlgorithmEngine contains 3 classes that you'll need to implement.

### IChromosome

Your chromosomes will need to implement the IChromosome class.

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

### ICrossoverManager

You'll need to implement the ICrossoverManager class. This tells the engine how to perform crossovers for your chromosomes.
You can read more about corossovers [here](https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)).

```CSharp
    public interface ICrossoverManager
    {
        IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2);
    }
```

### IPopulationGenerator

You'll also need to implement the IPopulationGenerator class. The engine uses this class to create its initial population. 
The PopulationGenerator will also renew the population when needed (see [IPopulationRenwalManagers](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/README.md#ipopulationrenwalmanagers)).

```CSharp
    public interface IPopulationGenerator
    {
        /// <summary>
        /// size is the number of chromosomes we want to generate
        /// </summary>
        IEnumerable<IChromosome> GeneratePopulation(int size);
    }
```

## Creating an Instance of GeneticSearchEngine

It's highly recommended that you use the [GeneticSearchEngineBuilder](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/GeneticSearchEngineBuilder.cs) class to create your GeneticSearchEngine. See the following example.

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
By default, the probability of mutations is 0. You can change this be using the GeneticSearchEngineBuilder.SetMutationProbability(double probability) method.
Note that the mutation probability will be ignored if you set a [MutationManager](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine#imutationmanager).

### CancellationToken
You can use the GeneticSearchEngineBuilder.SetCancellationToken(CancellationToken cancellationToken) method to add cencellationTokens.
The cancellation is checked once per generation, which means that if you're generations take a while to run, there may be a delay between your requesting of the cancellation and the engine actually stopping.

When the cancellation is requested, you'll get the result that was found up till than.

### IncludeAllHistory
If this option is turned on (by default it's off) the result will include the entire history of the population.

### Elitism
Using elitism, you can set a percentage of the best chromosomes that will be passed "as is" to the next generation.
You can read more about elitism [here](https://en.wikipedia.org/wiki/Genetic_algorithm#Elitism).

### IStopManagers
[StopManagers](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IStopManager.cs) let you configure when you want the search to stop. 
StopManagers can be added using the GeneticSearchEngineBuilder.AddStopManager(IStopManager manager) method.
You can create your own managers by implementing the IStopManager class, or use one of the existing managers.
Note that there is no limit to the number of StopManagers you can add to your search engine.

You can find a tutorial on creating a custom StopManager [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Creating-a-Custom-StopManager).
In addition, [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/tree/master/GeneticAlgorithm/StopManagers) are some examples of custom StopManagers.

Existing StopManagers:
- **StopAtEvaluation**: Will cause the search to stop when it reaches some predefined evaluation.
- **StopAtConvergence**: The search will stop when the difference between chromosomes in the population is too small.
- **StopIfNoImprovment**: Will stop if the improvement over 'X' generations isn't good enough.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddStopManager(new StopAtConvergence(0.5)).Build();

var result = searchEngine.Run();
```

### IPopulationRenwalManagers
[PopulationRenwalManagers](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IPopulationRenwalManager.cs) will tell the engine to renew a certain percentage of the population if some condition is met. 
PopulationRenwalManagers can be added using the GeneticSearchEngineBuilder.AddPopulationRenwalManager(IPopulationRenwalManager manager) method.
You can create your own managers by implementing the IPopulationRenwalManager class, or use one of the existing managers.
Note that there is no limit to the number of PopulationRenwalManagers you can add to your search engine.

You can find a tutorial on creating a custom PopulationRenwalManager [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Creating-a-Custom-PopulationRenwalManager).
In addition, [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/tree/master/GeneticAlgorithm/PopulationRenwalManagers) are some examples of custom PopulationRenwalManagers.


Existing PopulationRenwalManagers:
- **RenewAtConvergence**: The search will renew some of the population if the difference between chromosomes in the population is too small.
- **RenewIfNoImprovment**: Will renew some of the population if the improvement over 'X' generations isn't good enough.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddPopulationRenwalManager(new RenewAtConvergence(0.5, 0.5)).Build();

var result = searchEngine.Run();
```

### IMutationManager

The [IMutationManager](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IMutationManager.cs) class lets you dynamically determine the probability of a mutation based on the current population.
For instance, you might want to set a high mutation probability for a few generations if the population is homogeneous, and lower it while the population is diversified.

You can find an exsample of a custom MutationManager [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/MutationManagers/ConvergenceMutationManager.cs).

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetCustomMutationManager(new MyMutationManager()).Build();

var result = searchEngine.Run();
```

### IPopulationConverter

The [IPopulationConverter](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IPopulationConverter.cs) interface provides you with a very powerful tool for customizing your search.
The IPopulationConverter method ConvertPopulation is called every generation after the population is created. In this method you can change the population in any way you want.
This allows you to add [Lamarckian evolution](https://amitksaha.wordpress.com/2009/12/04/lamarckism-in-genetic-algorithms/) to your algorithm - that is, let the chromosomes improve themselves before generating the children.

There is no limit to the number of PopulationConverters you can add to your search. If you add more than one, they will be called in the order in which they were added.

Example:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddPopulationConverter(new MyPopulationConverter()).Build();

var result = searchEngine.Run();
```

You can find an example of a custom PopulationConverter [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/PopulationConverters/SamplePopulationConverter.cs).

## Using an Environment

Sometimes, it's impossible to evaluate a chromosome without knowing information about it's surroundings, such as the rest of the population. (This, by the way, in the case in nature - where the fitness an individual depends on its envierment and the way it interacts with the other individuals).

GeneticAlgorithmEngine provides two classes to deal with this.

### IEnvironment

The [IEnvironment](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IEnvironment.cs) represents the "environment". 
You can set your own environment. If you don't, we will use the [DefaultEnvironment](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/DefaultEnvironment.cs) class, which contains the other chromosomes, and the generation number.

The environment's UpdateEnvierment is called before the evaluation of a generation begins, which lets you configuration your environment. UpdateEnvierment is guaranteed to be called once per generation.

You can find an example of a custom Environment [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/EnvironmentGui/MyEnvironment.cs).

### IChromosomeEvaluator

If you set the [IChromosomeEvaluator](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/GeneticAlgorithm/Interfaces/IChromosomeEvaluator.cs), the engine will use your ChromosomeEvaluator's evaluate method (and not the chromosome's default evaluate method).
Since the IChromosomeEvaluator's SetEnvierment is called before the evaluation starts, your ChromosomeEvaluator can use the information in the environment to evaluate the chromosomes.

You can find an example of a custom ChromosomeEvaluator [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/blob/master/EnvironmentGui/ChromosomeEvaluator.cs).

### Example

```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.SetEnvironment(new MyEnvironment) // If you don't set an environment, we'll use the DefaultEnvironment class
	.SetCustomChromosomeEvaluator(new MyChromosomeEvaluator()).Build();

var result = searchEngine.Run();
```

### Tutorial

You can find a tutorial on using an environment [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/wiki/Using-an-Environment).
The tutorial's full source code (alone with a poorly designed GUI) is located [here](https://github.com/ZviRosenfeld/GeneticAlgorithmEngine/tree/master/EnvironmentGui).
