SELECT 1
FROM   sys.tables
WHERE  object_id = OBJECT_ID('{TABLE_WITH_SCHEMA}', 'u') AND temporal_type = 2