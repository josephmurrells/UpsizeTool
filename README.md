# Exact Data Upsize Interface Tool

## Introduction

A tool for automating the process of upsizing EXACT data to SQL ready for a conversion to Dentally. The tool will create a SQL database based on user input, run the parallel upsize, backup and drop the SQL database, it will then zip the backup and open the relevant page to upload the data.

## Purpose

Previously this was a completely manual process which was quite time consuming and open to error, the tool aims to greatly reduce the manual work involved in upsizing EXACT data as well as reducing the number of potential errors that could be made.

## Usage

Start by running the "SQLTemplateUI.exe". You will require a copy of the ParallelUpsizer located in the root directory of the tool.

1. Enter the practice slug.
2. Enter the path to SQL directory, this is where the .ldf and .mdf will be created, you can either manually enter this path or select it from the folder dialogue window by pressing the "Select SQL Folder..." button.
3. Enter the path to the EXACT data you with to upsize, again you can either manually enter this path or select it from the folder dialogue window by pressing the "Select EXACTData Folder..." button.
4. Select the environment the data is intended for.
5. The database size will be autofilled in based on the EXACT data folder selected, however you can manually adjust this.
6. You can optionally enter the Practice ID, if you do this the tool will give you the opportunity to open the correct page to upload your zipped SQL backup.

The SQL server field will remember the last entered value, however you can change to to reflect whichever SQL server you plan to use by entering the server name.

When ready you can then press "Run Upsize Process" and the tool will automatically run through the complete upsize process. It will prompt for user confirmation in regard to zipping the backup and opening the upload webpage.

You can also click the "Generate Template" button which will generate the correct SQL queries to run if you plan to manually upsize the data.
