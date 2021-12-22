# apidrones
Api Restfull

***Requirement of the app***
  1) .Net Core 2.1
  2) Mysql Server 


***To deploy***.

Extract the project into a folder.
Open a shell command and go to folder.
Inside the folder execute "dotnet publish -c release". This command will generate the project to run on server. The project you will find inside folder "bin/release/netcoreapp2.1/publish/". 


To run the project 
  Open file appsetting.json and change the ConnectionStrings with the new data to the database
  Change the CronStringExecute how you want the job to run.
  When this changes are ready execute "dotnet apidrones.dll" and the api is runnig local in the port   5000.


Routes to the api 

- registering a medication => http://localhost:5000/api/medication (method: POST,
	parameters in the body {
    				"Code": "OER_438",
    				"Name": "Salbutamol",
    				"Weight": "2",
    				"image": "/image/medication1.jpg"
				})

- registering a drone => http://localhost:5000/api/Drones (method: POST, 
	parameters in the body {
    				"Serial_number": "sE88EJJSSS_907",
    				"Weight_limit" : 50,
    				"Battery_capacity" : 80,
    				"droneModelId" : 1
				})

- loading a drone with medication items => http://localhost:5000/api/Drones/{codeDrone}/loading (method:POST,
	parameters in the body [
				 <<< Array with all madication items to load >>>
				  {"medicationCode" : "NOTR_438", "quantity" : 4},
				  {"medicationCode" : "ASTR_433", "quantity" : 2}
			       ])	 

- checking loaded medication items for a given drone => http://localhost:5000/api/Drones/{codeDrone}/medicationsloaded (method: GET)
- checking available drones for loading => http://localhost:5000/api/Drones/availables (method: GET)
- check drone battery level for a given drone  => http://localhost:5000/api/Drones/{codeDrone}/battery (method: GET)

The job runs automatically when api starts. Inside the folder where the api is, the CheckBatteryLevelsLogs.txt file will be create. 
