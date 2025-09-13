# Take-Home Task: Build a Simple Credit Approval API

## Background

The company handles credit approvals as part of its core operations. The typical
process begins with a user submitting a credit request through an internal portal.
The request is subsequently reviewed and either approved or rejected by an internal
reviewer.

This task involves developing the backend component of a simplified version of this
workflow.

## Objective

Develop a backend API that supports a basic credit approval process. The workflow
consists of:

1. Submitting a credit request
2. Reviewing a credit request
3. Listing existing requests

## Functional Overview

Users will enter credit request information in a web interface. The input includes:

- Full name
- Email
- Monthly income
- Credit amount
- Credit type (MORTGAGE, AUTO, PERSONAL)

Upon submission, the request appears in an administrative dashboard under the
"Pending Review" section. An administrator can then approve or reject the request.
Once reviewed, the request is removed from the pending list.

The backend must enforce validation logic to reject requests with unreasonable data
and store all submissions persistently for future reporting. Each request should be
tagged with a unique identifier (e.g., CRE- 20250801 - 0001).


## Scope of Work

Based on the description, develop a backend API that fulfils the described
functionality. Frontend and authentication implementation are not required.

The solution should include:

- A suitable data model
- Relevant API endpoints
- Input validation
- Business rule enforcement
- Data persistence (using either an actual or in-memory database)

## Required Features

**_1. Submit a Credit Request_**
    - Accept user input and save the request
    - Generate a unique request number (e.g., CRE- 20250801 - 0001)
    - Initialize request status as PENDING_REVIEW

**Validation Rules:**

- Credit amount must not exceed:
    o 500,000 for MORTGAGE
    o 50,000 for AUTO
    o 10,000 for PERSONAL
- Credit amount and monthly income must be greater than 0
- Email address must be valid
**_2. Review a Request_**
- Endpoint to approve or reject a request
- Record reviewer name and timestamp
- Requests should be immutable once reviewed

**Business Rule:**

- Approval must be denied if credit amount exceeds 20 times the monthly
    income


**_3. List Requests_**
    - Provide a list of submitted credit requests
    - Support filtering by status or credit type

## Technical Requirements

The implementation may use any recent version of .NET (6 or later). The following
technologies are expected:

- ASP.NET Core Web API
- Entity Framework Core (in-memory database is acceptable)
- FluentValidation or a comparable validation framework

Authentication is not required for this task.

## Optional Enhancements

- Swagger/OpenAPI documentation
- Unit or integration tests
- Docker configuration
- Use of vertical slices, MediatR, or similar architectural patterns

## Deliverables

The completed solution should be submitted as either a GitHub repository or a
compressed archive. Please include:

- Complete source code
- Setup and execution instructions (README file)
- Any assumptions made during development

If you run out of time, please submit your current progress along with a short
explanation of any missing or incomplete parts. We place more value on clean
architecture, maintainable code, and alignment with the requirements than on full
feature coverage.


