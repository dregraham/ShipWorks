UPDATE [Resource]
  SET [Data] = 0x1A,
      [Checksum] = 0x1A, 
      [Compressed] = 0,
	  [Filename] = '__purged_email_plain.swr'
  WHERE Filename = '__EmailCleanup.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x1B,
      [Checksum] = 0x1B, 
      [Compressed] = 0,
	  [Filename] = '__purged_email_html_swr'
  WHERE Filename = '__EmailCleanupHTML.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x1C,
      [Checksum] = 0x1C, 
      [Compressed] = 0,
	  [Filename] = '__purged_print_html.swr'
  WHERE Filename = '__PrintCleanup.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x1D,
      [Checksum] = 0x1D, 
      [Compressed] = 0,
	  [Filename] = '__purged_print_thermal.swr'
  WHERE Filename = '__PrintCleanup_Thermal.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x1E,
      [Checksum] = 0x1E, 
      [Compressed] = 0,
	  [Filename] = '__purged_label.png'
  WHERE Filename = '__ResourceCleanup_png.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x1F,
      [Checksum] = 0x1F, 
      [Compressed] = 0,
	  [Filename] = '__purged_label.gif'
  WHERE Filename = '__ResourceCleanup_gif.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x2A,
      [Checksum] = 0x2A, 
      [Compressed] = 0,
	  [Filename] = '__purged_label.jpg'
  WHERE Filename = '__ResourceCleanup_jpg.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x2B,
      [Checksum] = 0x2B, 
      [Compressed] = 0,
	  [Filename] = '__purged_label_epl.swr'
  WHERE Filename = '__ResourceCleanup_epl.swr' 
GO

UPDATE [Resource]
  SET [Data] = 0x2C,
      [Checksum] = 0x2C, 
      [Compressed] = 0,
	  [Filename] = '__purged_label_zpl.swr'
  WHERE Filename = '__ResourceCleanup_zpl.swr' 
GO
