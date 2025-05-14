# Day 8

## Sessions

### Morning 

- Transaction in stored procedure
- Cursors in Stored Procedure
- Availability of servers
- Non availability reasons
- Master and slave
- Streaming replication - WAL

### Afternoon

- Task based on replication server in a standby mode with primary one
- Back up in the sql
- runtime-config-replication 


## Transaction in stored procedure

- Create a transaction statement in a stored procedure and rollback when there is an exception occurs

## Cursors in Stored Procedure

Cursors inside the stored procedure, which becomes reuseable in other statements also and there is some benefits like

- Improved Performance
- More secured
- Reduced complexity 

## Factors of Server Availability

- High availability which is directly equal to minimizing the down time
- Replication server with redundancy
- Automatic recovering server

## Non Availability reasons

- System crash
- Network failure
- Hardware failure
- Maintenance Time

## Master and Slave

- Master is act as a Primary server while slave is a standby server which have a access to read only
- In case of master crashes/network failure/ hardware issue - `Slave will promote to master`
- In the case of `Huge Traffic` - we can use slave as a side along master for `Load balancing`

## WAL

- It stands for `Write-ahead logging` which ensures data durability and crash recovery
- Before postgresql writes data changes to actual database, , it first writes them to a log fil which is `WAL` log, even if the system crashes, postgresql can replay the wal to restore the exact state.
