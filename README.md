Synopsis

In a project, you usually have to handle standard data like lists of countries, states/provinces, or cities that has to be there without much of a change. If we take an example of a website, that data may end up on majority of pages and you may have to write similar or boilerplate code to make that data available there. The interaction of such data with user usually doesn't involve much complication: they select values applicable to them and move on.

We can call this data 'Reference Data'. In a medical project, reference data can include among many other things, a list of hospitals or pharmacies. In a transportation project, it might include a list of parking stations. Accessing this data and enabling user to work with it would mean you run a query, get all reference data and then present it to user who will select one option and you save that option.

This means you will write similar sort of query (with different entities of course), you'll write similar mechanism to enable your view to get that data and then present it to user. For a project with lots of reference data, that's a lot of work (and inefficient) writing same sort of code again and again and keep writing it whenever new reference data comes.



Break-down of Project's Structure

This project – written in .NET Core 3.1 – aims to fix this problem through C# (of course the other way is to design your database more cleverly, but if not, then read on) by writing a service and assisting DTO and ViewModels to enable your user-end client to get reference data as required. Let's first point out the layers:

- The DomainLayer will contain the entities. At the moment, it only has Reference Data related entities.

- The DataLayer can host your ORM communicating with database. For simplicity I am using a simple DataBank class that uses generic collections. By virtue of Abstraction, the services layer doesn't know whether it is calling an ORM or a custom implementation.

- The ServicesLayer will contain all the services that the project needs. At the moment it contains RepositoryService (with two methods) and ReferenceDataService,

- DataTransferObjectsAndViewModels cannot be considered a layer but a library in true sense that bridges the communication between services and clients (at the moment, one client is added i.e. MVC web app).

- Utilities is another library that contains code (like constants and extensions) usable by every layer or other libraries.

- MVC Client is the example end-user client that's added to demonstrate the end product.



Mechanism

The Reference Data is defined as distinct domain classes. The project uses Canadian provinces as prime example. This means that the project has an entity Province defined in the Domain layer.  

The best place for someone to know that a view needs a list of provinces in its form is that view itself. Instead of second guessing and anticipating when who needs what, if the person needing something (knows how to ask for it and) asks for it, that results in an efficient work-flow. This results in a clear separation of concerns and decoupled flow of data.
For this purpose, .NET Core's View Components' feature is used to success. Using a View Component, the view invokes the process by stating it needs a list of all colours. It can also provide criteria of selection e.g. it needs colours that start with 'R'; or the colours that got added after a specific date or are valid within specific time frame, and so on..  

Once the request is received by the component, it parses it and then submit the processed request to ReferenceDataService. The service then invokes Expression Trees to formulate a query and uses RepositoryService's generic methods to query the DB (or in this project's case, DataBank). The result is returned to the component which composes the HTML element around the received data and sends it back to the view which can render it as received.



Observations

1. For view to be able to provide correct criteria of how it needs Reference Data, look at SelectionCriteria class. Method chaining is employed for a fluent syntax for view to provide criteria. 

2. For view to provide which type of Reference Data it needs, it has access to an enum that lists all the reference data within the project. The ReferenceDataSerivce at one point translates all those enums to actual Type declarations of Reference Data.

The reason this route is taken is to keep Domain and Services layers not get intertwined in views. In other words, a view should not have to bother about Domain classes and Services and all that happens in between. It needs its data so that it can render it. It can also ask for a particular data (e.g. 'Get me the list of provinces please.') but without worrying about how it is implemented. This enum is maintained in the ProjectConstants class. We have to update this list for any type of Reference Data that we want to support on the client side. 

IMO, this is a small price to pay for otherwise a completely decoupled system where any client is capable of asking for a specific criteria of any type of reference data without any particular dependency.

3. Working examples are on Home Index. Run the project and you'll see it in action right away.
