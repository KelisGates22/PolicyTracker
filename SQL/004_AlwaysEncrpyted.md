# Always Encrypted on Customers.SSN

After running 001-003, encrypt the SSN column:

1. In SSMS, right-click **dbo.Customers** > **Encrypt Columns...**
2. Check **SSN**, encryption type **Deterministic**
3. Column master key: **Windows Certificate Store > Current User**
4. Run now (offline), then Finish

Verify: `SELECT FirstName, SSN FROM Customers;` should show unreadable bytes.

The API connection string includes `Column Encryption Setting=Enabled`,
so the API can decrypt with the certificate on the same machine.