IF EXISTS-- if temporal table exists
(
	SELECT 1
	FROM   sys.tables
	WHERE  object_id = OBJECT_ID('{TABLE_WITH_SCHEMA}', 'u') AND temporal_type = 2
)
AND
EXISTS	-- if the base table exists
(
	SELECT 1
	FROM   sys.tables
	WHERE  object_id = OBJECT_ID('{TABLE_WITH_SCHEMA}', 'u')
)
BEGIN
    ALTER TABLE {TABLE_WITH_SCHEMA} SET (SYSTEM_VERSIONING = OFF);

    /* Optionally, DROP PERIOD if you want to revert temporal table to a non-temporal */
    ALTER TABLE {TABLE_WITH_SCHEMA}
    DROP PERIOD FOR SYSTEM_TIME;

	ALTER TABLE {TABLE_WITH_SCHEMA}
    DROP CONSTRAINT {SYS_TIME_CONSTRAINT}_DF_SysStart, {SYS_TIME_CONSTRAINT}_DF_SysEnd;

	ALTER TABLE {TABLE_WITH_SCHEMA}
    DROP COLUMN SysStartTime, SysEndTime;

    DROP TABLE {HISTORY_TABLE_NAME};
END