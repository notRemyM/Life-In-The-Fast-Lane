# LifeInTheFastLane -- Dynamics 365 / Dataverse SDK performance tester
LifeInTheFastLane (named after the Eagles song) is a Dynamics 365/Dataverse SDK write performance testing application. 
Features include:
- Various methods of creating rows within Dataverse, including Create, CreateMultiple, threaded Create, threaded CreateMultiple and CreateMultipleAsync. 
- Performance tracking for the above methods
- Button to create batch job to delete records automatically
- Ability to change EntityCollection size generated as dummy data

## Directions for use
Please make sure you have the LifeInTheFastLaneDemoEntity solution provisioned on your target environment. 
You can then populate the connection string inside App.config.template with your own domain, client ID and client secret, then rename to App.config and run. 

## Credits
- Mark Carrington (https://markcarrington.dev/2020/12/04/improving-insert-update-delete-performance-in-d365-dataverse/) for his very helpful test code and article on fast Create and CreateMultiple.
- Microsoft for the Power Platform SDK
- Microsoft again for the Power Platform examples. These helped a lot in creating the application as well as the CreateMultipleAsync implementation, which is a near carbon copy of theirs. 