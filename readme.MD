# DaveAPI

15 Nov 2020

## Tools used

* .NET Core 3.1
* Visual Studio (not needed for running the service)

## How to run this web service

1. Download and install .NET Core 3.1 for your platform (Windows, Mac, or Linux)

2. In a terminal window, navigate to the /DaveAPI/DaveAPI folder in this codebase.

3. In the terminal window, run the command:

   ```
   dotnet run
   ```

4. From Postman, curl, or another web client, submit requests to the service, as described in the following section.

## How to format requests to the service

The service listens on port 5000 on your localhost. There are three endpoints:

### POST /api/addition

Performs the Add operation.

Payload: A JSON array of objects that specify the additions to be performed.

Returns: nothing

Example Invocation:

```
curl --location --request POST 'http://localhost:5000/api/addition' \
--header 'Content-Type: application/json' \
--data-raw '[
    {
        "User": "myUser",
        "Payer": "DANNON",
        "Points": 300,
        "Date": "2020-10-31T10:00:00Z"
    },
    {
        "User": "myUser",
        "Payer": "UNILEVER",
        "Points": 200,
        "Date": "2020-10-31T11:00:00Z"
    },
    {
        "User": "myUser",
        "Payer": "DANNON",
        "Points": -200,
        "Date": "2020-10-31T15:00:00Z"
    },
    {
        "User": "myUser",
        "Payer": "MILLER COORS",
        "Points": 10000,
        "Date": "2020-11-01T14:00:00Z"
    },
    {
        "User": "myUser",
        "Payer": "DANNON",
        "Points": 1000,
        "Date": "2020-11-02T14:00:00Z"
    }
]'
```

### POST /api/deduction

Performs the Deduct operationon.

Payload: A JSON object that specifies the deduction to be performed.

Returns a JSON array indicating the results of the operation.

Example Invocation:

```
curl --location --request POST 'http://localhost:5000/api/deduction' \
--header 'Content-Type: application/json' \
--data-raw '{
    "User": "myUser",
    "Points": 5000
}'
```

Example Response:

```
[
    {
        "payer": "DANNON",
        "points": -100,
        "date": "2020-11-16T04:38:38.736786Z"
    },
    {
        "payer": "UNILEVER",
        "points": -200,
        "date": "2020-11-16T04:38:38.736786Z"
    },
    {
        "payer": "MILLER COORS",
        "points": -4700,
        "date": "2020-11-16T04:38:38.736786Z"
    }
]
```

### GET /api/balance?user=<username>

Performs the Balance operation.

Example Invocation:

```
curl --location --request GET 'http://localhost:5000/api/balance?user=myUser'
```

Example Response:

```
[
    {
        "payer": "DANNON",
        "points": 1000
    },
    {
        "payer": "UNILEVER",
        "points": 0
    },
    {
        "payer": "MILLER COORS",
        "points": 5300
    }
]
```

## Overview of codebase

* The Models folder contains four DTO classes: Addition, Deduction, DeductionResultItem, and BalanceItem to provide input/output for the service endpoints.
* This folder also contains the class, DataStore, which provides the in-memory data store in addition to the operations on it.
* The Controllers folder contains the endpoint controllers, which just forward operations into the DataStore methods.
* See the code comments for DataStore.AdjustedAdditionHistoryByUser for an explanation of how data is manipulated.

## How I would turn this into production code

* Add unit testing.
* Add logging.
* Use a tool to generate API documentation from the code.
* Analyse failure modes and add error reporting / mitigation. The current code is happy path only.

## Other notes

* It appears that there is an error in the example in the specification. I think that the text that says "[MILLER COORS, -4,000 points, now]", should have said -4700 points.
* I elected not to detect whether additions were done in mixed-up date order, as the behavior in this case wasn't specified. That means that the order of the records is the order of addition, not the order of the date values. The date values are just data.
