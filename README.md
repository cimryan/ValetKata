# The World’s Most Awesome Parking Lot Manager Application

An exercise to introduce the concept of simulation in contrast to mocking.

## Preliminaries
This exercise works best when you have access to an instance of Sql Server. You can use Sql Server Express if you don’t already have a Sql Server instance lying around.
There is a script in the project called CreateValetDatabase.sql which will create the database and all the objects required by the initial project.

The initial implementation uses the packages xUnit.net, xUnit.net Extensions, and Fluent Assertions, which can all be installed using NuGet. If you’d like to run the tests in the Test Explorer (recommended) you should download and install “xUnt.net runner for Visual Studio 2012 and 2013” from the Visual Studio Gallery instead of using NuGet.

The code is not Clean. The original developer apparently decided that input validation was a passing fad. The unit tests are incomplete. The UI is tightly coupled to the object model. Consider these additional challenges to be surmounted, but don’t let them distract you from the primary goal of the exercise.

## The Exercise
You'll find a simple Valet application. You'll note that the business logic layer has a set of unit tests. Run them and note that the server appears somewhat overtaxed.

Your primary task is to decouple the code from the database so that it won't be a chore to run the unit tests:

1.	Encapsulate the database access in an abstract class or interface.
2.	Change the unit tests so that you can easily run them against any implementation of the interface or abstract class.
3.	Use the unit tests to incrementally develop your alternative implementation.

Now, use your simulated storage system to add new functionality the application. Specifically, add the capability to look up the parking spots of all cars having a specific make, model, and color. Can you use the (awesome) UI with your implementation?

How can you make it easy to switch between implementations of the storage system in the application? Can you make it possible to switch in production without re-deploying?

## After-action discussion
The pattern of replacing one implementation with another which is useful in a restricted context has been called “simulation”, with the implementation called a “simulator”*.

Your implementation probably doesn’t account for concurrent access to the storage system, and it probably doesn’t account for persistence across invocations of the application. These are the restrictions being placed on the context in which the simulator is valid, and those restrictions are perfectly reasonable for the purpose for which the simulator was created: To facilitate testing.

The fundamental advantage of using simulation instead of mocking is that drift between the implementations can be automatically detected. When using a mock object the test specifies the behavior or the object, with no connection to the real class. If the implementation of the real class changes the test can continue to pass even though the production code will fail. When using simulation the same tests can run against both implementations, providing you an automated way to detect these issues. These sorts of tests have been called "Contract tests".

This pattern is not always appropriate. For example, if there were a complicated stored procedure in the database that generated cryptographically strong ticket numbers using some bespoke algorithm then simulating that algorithm would probably require wholly re-implementing it. However, even in these cases some simulation may be feasible: Perhaps the majority of the tests, which presumably don’t care about the cryptographic strength of the ticket number, could run using the simulator, while the few that cover this facet could use the real implementation. Or perhaps the real implementation could be factored such that the simulator could make direct use of the cryptographic algorithm.

When using a tell-don't-ask-design you'll frequently end up with classes at the leaves of the model which have unobservable side effects. That is to say, the side effects will be unobservable through the interface. If the contract tests are to be run against multiple implementations then can they be used to verify that a particular implementation has performed one of these unobservable side effects, or do you need other tests? Do those tests belong in the same project as the contract tests? Does the component they test belong in the same project as the code that uses it?

Finally, an interesting point to consider is that the simulator is a legitimate implementation of an abstract concept. I.e. you expect the simulator to work with untested inputs. (Contrast with a mock.) This means that if you can find uses for your simulator in the production code there’s no reason not to use it. For example, could you implement a serializer for the simulator you developed, here? Or perhaps use a DataContractSerializer for it? Could you serialize the simulator to disk and load it from disk, so you didn’t need a database server, at all?

---

\* This term was coined by [Arlo Belshee](http://arlobelshee.com/mock-free-example-part-2-simulators)
