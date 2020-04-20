# myfinances
Personal Finance API that integrates with you bank


Not using aync flow for now. Useful if large amount of data being returned
The transaction time is being persisted as it is. Timezone information is not handled. Also the querying transaction method does not account for timezone.
Summary calcuation does not handle case where account have different currency currently. Assuming all accounts have one currency. No handling of conversion and currency rates
