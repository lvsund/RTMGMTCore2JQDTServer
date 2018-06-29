---
title: "Reports To Fixit Application"
author: "Lyndon Sundmark"
date: "June 23, 2018"
output:
  html_document: default
  word_document: default
  pdf_document: default
---



# Introduction

Welcome to the Reports-To Fixit Application. 

![Main Page](../master/docs/MainPage.jpg?raw=true)

The purpose of this application is to assist organizations in maintaining the information often needed for:

* organization charts and 
* security to information access 

and to keep it in good order.

## Why Is This Needed?

My background over the last 30 -35 years has been in a combination of Human Resources and Human Resource Information Technology roles. During that time, I noticed that organizations almost always:

* have a need to produce organization charts efficiently and
* have a need  for their HR  Information Systems to show each manager only information on  their people and no other part of the organization. And that isnt only one level below but all levels below

Sometimes this functionality is already provided in the applications themselves and can be quite robust. Other times- not so much. Sometimes the functionality provided is little more than providing both a person or position identifier and the person or position that the person or position reports to. Sometimes its through 'financial' control codes where there is a tie into financial systems.

If it's based on who reports to who,You might think that having each employee record having information on who they report to is sufficient. But for the most part it is not. Unless the application requires the reports to information to be completed, it can:

* often be left blank when people are in a hurry. 
* Or you can have the person reported to leave the company, and the  'reporting-tos' are left orphaned. 
* Or you can have reporting to information even become circular- someone above accidently reporting to someone below.

 Blanks and missing information are easy to spot as errors. But when information is filled in but not correct -how do we know?

Bottom line- no matter how good the technical side of this technology is -human error can often step in- making much of the usability moot. I have seen many variations  on what could go wrong- but definitely have not seen all of them.

## Why Create This Application?

With that in mind, I wanted to create an application that could help organizations at least see where many of their errors in their reports-tos information are occurring as a means to assist them in cleaning this information up.

As with any application there is a limited scope of what this application is intended to do. It is not the be all and the end all and will not solve every reports to issue you will find. But will certainly get you well along the way to having a much better picture of what is going on in this area of your information management and assist you in correcting this information for the uses it is intended for.

## What This Application Is Intended For

The intent of this application is to help organizations address the 'technical' side of reports to information ( I will elaborate momentarily on what 'technical' means here).

## What This Application Is Not Intended For

This application is not intended to guarantee 'accuracy' of the reports to information. 

Even if there is nothing technically wrong with your reports to information ( no structural broken reports to links), it does not guarantee that the information for the reports-tos is up to date and correct (accuracy). That is always a human administrative issue, and is clearly outside the scope of the intent of this application. The information can technically work but not be up to date for example.

## How To Know When Your Reports-To Information Is 'Technically' Correct

For reports to information to be 'technically' correct, at least a couple of criteria come to mind:

.	Everyone in the organization must eventually report to the top person in some fashion (hierarchical pyramid structure)
.	Everyone in the organization that should be included must be included

These may sound like simple guidelines. But for the most part, when things go wrong with reports-to information- it is because of one or both of these reasons. 

__Paying attention to these is what this application does.__

# Features

*	This application is a standalone application. It processes reports to information. The application doesn’t care where the information comes from. An organization could:

    * keep this information in a spreadsheet, or 

    * enter it directly in this application or 
    
    * have it come from a Human Resource Information System. 

*	This application allows for direct entry of information or for import of information. 
    * If an organization wants to refresh the information automatically nightly from another source such as a Human Resource Information System without manual import- there is nothing preventing an organization from doing that with SQL Server Integration Scripts.But it would write over any local changes you made, unless those were input into source system the night before. Importing information is probably best because it is relatively effortless and under your control.

*	This application provides a number of different summaries of the reports-to data:

    * A 'Reports To Records' summary- what the information is before any processing is done

    * A 'Top Down Processed Records'- showing all the records that correctly report to the top ( no broken links)

    *	A 'Bottom Up Processed Records' showing records not reporting to the top and where they break. These are also the broken records

    * A  'Fix Records' summary showing which of the broken records  need to be corrected. ( Not all of them do as will be explained further below)

    * A means by which to make the corrections- Required Corrections.

    * The ability to export information out of the application including:
      
      * Correct processed reports to records
      
      * Corrections needing processing – this allows source systems outside of this application to have information need to make corrections whether by batch or manually
      
      * Exporting out the  Top Down Processed records- because sometimes other applications need this type of information.

* This application allows corrections to be tested locally before having corrections being given back to source systems. A nightly background auto update would mean you would have to wait for the next days refresh, if your local testing of corrections didn’t give you what you were expecting). This will make more sense later in this documentation.

* The application is web based. This means that the application can render on most browsers.


# Walkthrough

## What Information Is Required

Lets answer this by taking a look at the first choice on the main page shown previously- the 'Reports To Records':

![Reports To Records](ReportsToRecords.jpg)

The application requires 5 basic pieces of information:

1.	Reporting ID

2.	Title

3.	Name

4.	Reports To ID

5.	Employee ID


### Reporting Id And Reports To Id

The Reporting Id is the identifier for a 'person' or 'position'. This requires an immediate elaboration. 

This application can use either employee information or position management information for ‘reporting’ relationships. 

* Some organizations use HR employee information to store reporting relationship information ( who reports to who) , and 

* some use position information to store what position reports to what position. 

This latter approach is a form of 'position management'. In position management reporting relationships are based on positions. People move in and out of positions , but unless the organizational structure changes , the reports to information remains the same.

I think in many organizations , structural changes change far less often than people coming and going. In employee information based reporting relationships, every time some one gets hired or leaves the organization, reporting relationships change and have to be kept up to date. Under position management based, only when organizational structural changes occur do you change the actual reports to information. You do have to change employee information related to positions as people move in and out though. Hopefully what is being described here makes sense.

In any case, this application doesn’t care. By using the terminology ‘Reporting ID’ and ‘Reports to ID’, either employee based or position based structural information can be used. 

* If employee based, the reporting id would likely be the employee Id or number. The  reports to id would be the employee id or number of the person they are reporting to.

* If position based, the reporting id is the employees position number, and the reports to id is the position number that the position is reporting to.


### Title And Name

The title and name are needed to give the user a recognizable ‘visual’ on who and what we are dealing with. The title is  the position title ( if position based) or the job title (if employee based), and the name  of the person.  The Name and Title you would normally expect to find as contents in most organization chart applications.

### Employee Id

Regardless of whether you are using position based or employee based reporting relationships, the use of this information outside of this application for other purposes often requires linking up  together employee information. By including this, it allows for that. IF you are using employee based relationship reporting ( who reports to who) then the reporting id and  the employee id are the same information.




## Getting Information In

### Importing Data

When you look at the Reports To Records screen above you will notice some choices represented as hyperlinks, The first of these is 'Import Records'. This choice expects a csv file formatted with appropriate contents shown below:

As text:

![CSV Text](csv data.jpg)

As Excel spreadsheet saved as csv file:

![Excel CSV](excelcsv.jpg)

When you click that link you get the following screen and prompt:

![Import Prompt](importScreen1.jpg)

You  will then be prompted for the data file itself:

![Import Prompt](importScreen2.jpg)

Once you have clicked import, the data will appear in the grid.


### Manual Entry

A second way of getting data in is manual data entry.You will notice on the ReportsToRecords screen, that there is a create link to manual enter new records and edit, details, and delete links to carry out those actions. 

#### Create

![Create Screen](create.jpg)

You will notice you dont have to enter an 'id' field- that is automatically generated by the application.

#### Edit

![Edit Screen](edit.jpg)

#### Details

![Details Screen](details.jpg)

You will notice that the Details screen is 'read only' just for viewing

#### Delete

![Delete Screen](delete.jpg)

Finally - you will notice the delete prompts you appropriately for confirmation before deleting a single record.



### Background Nightly Updates


This option is probably the most elegant of the 3 means to populate your reports to information. It would be based on the assumption that you are reviewing and correcting the information daily and passing the corrections back to who and where they are done in source systems , so that the next day you receive a new update of reports to records with corrections in place. It means you are giving up control of being able to import the data at your convenience.

## Processing The Information And Reveiwing The Results


### What You Need To Do Before Processing Records

There is one final step you need to do or ensure when you want to process the records. The person who is at the top of your organization must have their Reports To ID set to ‘TOP’. This is how the application knows how where to start its processing of the records.


![Reports To Records](ReportsToRecords.jpg)

In the sample data 'Lujan' is the top person in the organization

Then you  press the ‘Create Reporting Hierachy’ link and await its completion:

![Create Hierarchy](CreateHierarchy.jpg)

### What Actually Happens In Processing The Records

Once you run the ‘Create Reporting Hierachy’  a  bunch of processing occurs behind the scenes.


#### Top Down Hierarchy

This first step in the processing is creating a top down hierarchy. Starting from the TOP the processing asks who reports to the TOP ( it’s the ‘Reporting ID’ that has the ‘Reports To ID’ = ‘TOP’). It then says ‘ok’ who or what position(s) reports to that ‘Reporting ID’. It then goes down to the next level and asks the same question. This goes down every branch recursively / reptatively ( from the top) in the organizational structure until there is no further level to go down to.

When this process is completed, it has found ‘all the records that can successfully able to be linked back to the TOP'. __This many not be ALL the original  reports to records that were originally entered imported etc.__ If ANY original Reports To Records have a Reports to ID that does not itself eventually (even through several levels)report back to the TOP- then they are not included. The above process starts at the TOP and works its way down finding all that do eventually report to it.

These records show up on the page Top Down Hierarchy:

![Top Down Hierarchy](TopDown.jpg)

There are a couple of  things you should notice here:

*	A reporting ‘level’ field is provided to you that shows how many levels from the top the position or person is .
	
*	It also shows an RT_STRING ( reports to string) that shows how that position or person links back to the top.
	
*	You will also notice on the lower right bottom that the application allows you to export out these top down processed records. Often external applications to this might need this type of information to control who sees what based on reporting relationships. Pressing that button gives you a prompt at the bottom of the page giving access to the tab delimited file created:


•	Clicking open will give you a view of  the data. Once again the prompt will look different depending on the browser you are using: 


Again- all of the records  that successfully are linked to the top with their reports to information are on this summary. If the total count of employees in the top down processed records is the same as the count of the original reports to records, then all of  them are 'technically' CORRECT.

#### Orphan Records Identification

As mentioned previously, unless you are very good at keeping your reports to information accurately and correctly up to date so that ‘EVERYONE’ reports to the TOP, it is likely that there will be some records that were in the original reports to records list wont be in the top down hierarchy list.

These would be regarded as broken or orphaned records because they don’t report to the TOP. The processing easily identifies these by comparing the original list  to the top down processed list and identifies those that are not in the top down processed list.

The reason they are orphaned or broken is because the ‘Reports to ID’ information is broken somewhere on the way up to the top. It doesn’t matter what the variety of mistakes is to the contents of this field are. The fact that the TOP DOWN HIERARCHY recursion didn’t pick them up is what makes them that. To see these records you can press the link ( Bottom Up Processed Records) on the Main page to see these:

![Bottom Up Records](BottomUp.jpg)

If you take a close look, at the RT-STRING - you will notice that all of these records have the same number in this example data of 9999 at end of the string. In the bottom up processing, the RT-STRING is built bottom -up /left -right.

ALL of the above records are ‘broken’ in the sense that they are not reporting eventually to TOP ( 1558 and 9999 instead).
BUT not all these records have to be corrected. In this example its likely that all those records  do accurately report to 1558, but the Reporting_ID record of 1558 is likely the only record that needs to be fixed.

How do we know which of the broken records to fix?

That brings us to this next topic.



#### Record Correction Identification

In order to identify which records actually need correction, the processing takes all those records that were broken and starts mapping upwards until it cant go any further. At that point, whatever the ‘Reports To ID’ is  when it cant go any further, the position(s) that  has(have) that reports to ID as its REPORTING_ID are the ones that actually need to be fixed. Lets look in detail at each of these:

##### Broken Records

![Broken Records](Broken Records.jpg)

Broken Records shows the 35 records in this data that are orphans, but this doesnt mean that we need to make changes to those 35 records themselves. They all suffer from the same issue in this case- reporting to 1158 which in turn reports to 9999 which doesnt report to anything because in this case 9999 doesnt exist as a 'Reporting_id'.

To determine how many records have to 'actually be fixed' we look at the 'Records To Be Fixed' page

##### Records To Be Fixed

![Records To Be Fixed](FixRecords.jpg)

Sure enough- the processing indicated that only ONE record needs to be fixed to ‘correct’ ALL the records that were listed in the Broken Records summary. When that one has its reports to id changed to be something that is correct- then all other records will not be broken the next time the hierarchy is generated.


The point here again is that what makes a broken or orphan record what it is , is that it doesn’t ultimately link back up to the TOP of the organization. NOT ALL broken pr orphaned records require corrections. Only those where the actually breakage occurs. And this is what the above processing summary does. It takes the hard work out of figuring out where to make the corrections.

##### Required Corrections



As you have seen your initial locus of activity has been from the first link on the main page;

![Main Page](MainPage.jpg)

Now a number of the other links on the main page are important- the next of which is the Top Down Processed Records


### How Long Does Processing Take?

All this processing above takes a bit of time. My own testing seems to show about 1000 records per every 3 seconds get processed. 17000 records take about 54-57 seconds as an example.

We tend to expect instant results when we press a button. And for many buttons in this application the processing is only 3-4 seconds.  This processing is different because so much processing is going on in the background. So please know ahead of time that the creation of the reporting hierarchy and identification of broken records and records requiring fixing takes a bit of time.


## Making Corrections

As a convenience, the processing takes those records that need to be fixed and populates the REPORTING ID and REPORTS TO ID into a separate summary and page:

![Required Corrections](RequiredCorrections.jpg)

The reason for only giving those pieces of information is that typically that is all that is needed at the source system to correct and update the information in the source system. You change the REPORTS TO ID on the records on this page so that they are the correct ones.

You will also notice some links on this page for testing, clearing, and exporting your corrections:

* Testing Your Corrections

* Clearing Corrections

* Exporting Corrections



### Testing Your Corrections

IF you are using the Import Records link, you can refresh under your own control ( if necessary). If you have this feature set on, andt you don’t care whether you blow away the original Reports To Records you started with- this feature will take your corrections on this page and update the Reports To Records locally with these corrections. You can then re Create the Reporting Hierarchy locally and see whether your corrections resulted in eliminating all broken or orphaned records. The catch is that your reports to records are no longer what you imported but rather what you imported plus corrections. If you made a mistake you can re import your records and rerun the creation of the hierarchy etc, but then you may have to make changes in the Required Corrections page. If your means to get the reports to records into this application is a nightly background script- you would need to wait overnight with fresh data to start the  processing again.

Bottom line –you need to decide whether you want to test corrections locally or not.
 

### Clearing Your Corrections

Clearing corrections simply clears the correction summary and blanks it out for the next time you process your reports to records.

### Exporting Corrections

What was mentioned at the beginning of this documentation, is that this application assumes it is stand alone. That means that it doesn’t care what the source or destination of the reports to information is. It provides means to import in information and export out.

The assumption of this as it applies to corrections is that if you have done work of processing the reports to records and made the appropriate corrections, you would like to export it out and hand off to those who need to make corrections in source systems.

The corrections are exported here out to a CSV file for external usage:

You can then pass that file off to those responsible for updating the source information systems.

## Miscellaneous

The application- when it does its hierarchy processing, first looks for duplicates. While this doesn’t necessarily prevent the processing from working, duplicates can create very spurious results. SO- these are shown to you on this page- for external source changing, but are deleted automatically from the local reports to records.

A duplicate is where there is more than one record where the combination of REPORTING_ID and REPORTS_TO_ID that are the same. This application and generally good reports to information should never show more than one reports to or boss for a person or position. IF this is a reality in your organization, for this application chose a ‘primary’ one.


# Closing

## Support

None. This application is released on a ‘user assumes any and all responsibility for its use’. This has been developed simply from a hobby perspective to test out various technologies.

## License

Reports To Fixit Application

Copyright © 2018 Lyndon Sundmark

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  

See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.



## Credit Where Credit Is Due


Many examples of code freely given on the internet were used to figure out how to do the various features including how to execute stored procedure from .net, how to import and export files etc. My thanks to all those who share snippets of code so that the rest of us can learn.

