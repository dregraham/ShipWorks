select t.name as table_name, c.name as column_name, c.seed_value
  from sys.tables t left outer join sys.identity_columns c on c.object_id = t.object_id
  order by seed_value