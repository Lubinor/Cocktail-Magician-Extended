#Task:
Extend the Cocktail entity with CreatedBy property. This property should hold the user that created the cocktail.
The UI should be extended to include the new property and also you should be able to search/filter by it.

#Work description:
The main DB entity for Cocktail has 2 new properties - User property, called "Creator" (felt more natural than CreatedBy, so I left the second naming version in the UI only), and CreatorId to form the One-To-Many relationship between the Cocktail and the User (respectively, the User class has a collection of CreatedCocktails). The new relation is also configured through FluentAPI, the seeder has been updated with CreatorId for the seeded cocktails.

All the other entities (DTO in the service layer and ViewModels in the web part), as well as all the Mapper classes have been updated with regards to the new property.

The List/Get methods in the Cocktail Service now include the Creator as well. The search and sort logic has been implemented in the service. 

In the Cocktail Controller, the Create action features an extra line to get the Id of the current user who is creating the cocktail, and assigns it to the newly created cocktail.

The DataTable for Cocktails (the index) and the individual Details screen for each Cocktail have their UI updated to display the new property.