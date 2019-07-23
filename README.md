# GeneticAlgorithmEngine

GeneticAlgorithmEngine provides an engine for running a Genetic Algorithm that can be easily configured to fit most search domains.

GeneticAlgorithmEngine contains 3 main classes that you'll need to implement.

### IChromosome

Your chromosomes will need to implement the IChromosome classes.

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

```CSharp
    public interface ICrossoverManager
    {
        IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2);
    }
```

### IPopulationGenerator

You'll also need to implement the IPopulationGenerator class. The engine uses this class to create its initial population. the PopulationGeneratorwill also renew the population when needed (see the secssion on IPopulationRenwalManagers).

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

It's highly recommended that you use the GeneticSearchEngineBuilder class to create your GeneticSearchEngine. See the following example.

```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, new NumberVectorCrossoverManager(),
     new NumberVectorBassicPopulationGenerator()).SetMutationProbability(0.1).Build();

var result = searchEngine.Search();
```

## Search Options

### Mutations

By defualt, the probability of mutations is 0. You can change this be using the GeneticSearchEngineBuilder.SetMutationProbability(double probability) method.

### IncludeAllHistory

If this option is turned on (by default it's off) the result will include the entire history of the population.

### IStopManagers

StopManagers let you configure when you want the search to stop. StopManagers can be added using the GeneticSearchEngineBuilder.AddStopManager(IStopManager manager) method.
You can create your own managers by implementing the IStopManager class, or use one of the existing managers.

Existing StopManagers:
- **StopAtEvaluation**: Will cause the search to stop when it reaches some predefined evaluation.
- **StopAtConvergence**: The search will stop when the difference between chromosomes in the population is too small.
- **StopIfNoImprovment**: Will stop if the improvement over 'X' generations isn't good enough.

Exsample:
```CSharp
var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager, populationGenerator)
	.AddStopManager(new StopAtConvergence(0.5)).Build();

var result = searchEngine.Search();
```

### IPopulationRenwalManagers

PopulationRenwalManagers will renew a certain percentage of the population if some condition is met. PopulationRenwalManagers can be added using the GeneticSearchEngineBuilder.AddPopulationRenwalManager(IPopulationRenwalManager manager) method.
You can create your own managers by implementing the IPopulationRenwalManager class, or use one of the existing managers.

Existing PopulationRenwalManagers:
- **RenewAtConvergence**: The search will renew some of the population if the difference between chromosomes in the population is too small.
- **RenewIfNoImprovment**: Will renew some of the population if the improvement over 'X' generations isn't good enough.
