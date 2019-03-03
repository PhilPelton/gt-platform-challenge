Before running, open Config.cs and change the following values:
	- csvFileLocation:  Set this to the full path of the location of the input csv file, including the file name and file extension.
	- webHookURL : set this to the full web path to the URL that you will be using on webHooks.
	- timeBetweenWebPosts : Change this if you want the program to run a bit faster (or a bit slower!).  Setting it too low will result in the webhooks website refusing the post due to volume of requests

	Run with CTRL + F5 if you don't want to miss console output.

	Assumptions for this project:
	These would normally be the kind of things that would get thrashed out before a spec is finalised, but the purposes of this execrices, I am assuming the following:
		- The task requires one row at a time from the input file, it seems sensible to read it all at once and store the data for easier access, since the volume of data is limited to 100 rows.
		  There doesn't seem to be a good reason to keep accessing the file and the danger of the data changing suggests that a snapshot is better.
		- The task requires processing the input file to stop after 100 lines.  I'm taking this to be that processing stops because the input file has run out of data after 100 lines, rather than 100
		  being a hard limit meaning if there are 150 lines, the program should stop after 100 anyway.
		- The task requires one action every 5 seconds.  I'm taking that to be an approximate value.
	
	Approach to task
	================

	First thing to do is read through the spec and understand the overview of the task.
	I haven't used github online before so the next step was to download and install github and then do a quick search online for the best windows gui.
	Once I'd got the csv file, I had a look at the data and noted that the first line contains headers and the final line is empty.  Both of these lines will need to be discarded when processing.
	Also of note is that one of the data entries contains a comma, which might be tricky.

	Since the task does not require any particular UI, a console application seems simplest.

	I split the project into three tasks: firstly reading data from the file, secondly converting it to Json and thirdly posting it to webhook.

	Read CSV file
	=============

	Reading a file is straightforward, although picking one line from a data file at a time every five seconds seems unnecessarily complicated and could lead to problems if the data is changed during
	processing.  So I took a decision to read all the data at one time and store it in memory.
	The issue was then how to deal with the data item containing a comma.
	I looked at three approaches:
	- install a reference to Microsoft.VisualBasic.FileIO.TextFieldParser, and use that to read the file.  I'm not sure about putting VB libraries into a c# project, that would definately 
	  have to be something I would look for in the company standards, or ask about
	- I found this: https://github.com/22222/CsvTextFieldParser in Nuget, which claims to do the job, but didn't work straight out of the box.
	- The method I've used, treating the data item as a special case.  Its not ideal, because there might be other data items in the future that also have comma in them.  However, I would rather
	present a working solution with a known issue than not complete the task.  In a work situation, I would bring this issue to the standup and we would discuss the advantages of a proper generic solution
	against the time necessary to get it working.  In this case, I'm on a deadline so this is it.

	Convert to Json
	===============

	Installing Newtonsoft.Json from Nuget takes care of this part easily.  If this were a larger project, I might move the createJsonString method to a utilities file, if it seemed appropriate.

	Post to webhook
	===============

	Once the data is loaded in and converted to json, it needs posting to webhooks.  Sending too many requests at once results in webhooks refusing to process the request.  Adding the 5 second delay
	makes it all run smoothly.

	Program structure
	=================

	The program is split into several classes, each having a single broad responsibility.
	Config - contains data that will need to be changed to run this challenge on a different machine, or using a different webhooks page.
	csvInterface - contains code that deals with reading in the data from the csv file
	Logging - critical functions are wrapped in a try-catch block, which sends details of the error and the calling method to the logging function.  This prints detials of the error to the console window,
	but could easily be ammended to store errors in a database or contact the dev team directly.
	Models - contains the structure of the data in one lne of the csv file.  It has its own class because it is used in both interfaces.
	Program - calls both interfaces to preform the three steps details above.
	webhookInterface - taks care of sending data to the webhooks website.  It will take all of the data from the input file and iterate over it.