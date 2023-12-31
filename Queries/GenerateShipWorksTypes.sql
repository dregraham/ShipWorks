IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShipmentType]') AND type IN (N'U'))
DROP TABLE ShipmentType
GO
CREATE TABLE ShipmentType (ShipmentTypeID int NOT NULL, Name varchar(50), CONSTRAINT PK_ShipmentType PRIMARY KEY (ShipmentTypeID))
GO
INSERT INTO ShipmentType (ShipmentTypeID, Name) VALUES
(0, 'UPS'), 
(1, 'UPS (WorldShip)'), 
(2, 'USPS (Endicia)'), 
(4, 'USPS (w/o Postage)'), 
(5, 'Other'), 
(6, 'FedEx'), 
(9, 'USPS (Express1 for Endicia)'), 
(11, 'OnTrac'), 
(12, 'i-parcel'), 
(13, 'USPS (Express1)'), 
(14, 'Best Rate'), 
(15, 'USPS'), 
(16, 'Amazon Seller Fulfilled Prime'), 
(17, 'DHL Express'), 
(18, 'Asendia'), 
(19, 'Amazon Shipping'), 
(99, 'None')
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceType]') AND type IN (N'U'))
DROP TABLE ServiceType
GO
CREATE TABLE ServiceType (ShipmentTypeID int NOT NULL, ServiceTypeID int NOT NULL, Name varchar(50), CONSTRAINT PK_ServiceType PRIMARY KEY (ShipmentTypeID, ServiceTypeID))
GO
INSERT INTO ServiceType (ShipmentTypeID, ServiceTypeID, Name) VALUES
(0, 0, 'UPS® Ground'), 
(0, 1, 'UPS 3 Day Select®'), 
(0, 2, 'UPS 2nd Day Air®'), 
(0, 3, 'UPS 2nd Day Air A.M.®'), 
(0, 4, 'UPS Next Day Air®'), 
(0, 5, 'UPS Next Day Air Saver®'), 
(0, 6, 'UPS Next Day Air® Early'), 
(0, 7, 'UPS Worldwide Express®'), 
(0, 8, 'UPS Worldwide Express Plus®'), 
(0, 9, 'UPS Worldwide Expedited®'), 
(0, 10, 'UPS Worldwide Saver®'), 
(0, 11, 'UPS Standard'), 
(0, 12, 'UPS First Class Innovations®'), 
(0, 13, 'UPS Priority Mail Innovations®'), 
(0, 14, 'UPS Expedited Mail Innovations®'), 
(0, 15, 'UPS Economy Mail Innovations®'), 
(0, 16, 'UPS Priority Mail Innovations®'), 
(0, 17, 'UPS SurePost® Less than 1 LB'), 
(0, 18, 'UPS SurePost® 1 LB or Greater'), 
(0, 19, 'UPS SurePost® Bound Printed Matter'), 
(0, 20, 'UPS SurePost® Media'), 
(0, 21, 'UPS Express®'), 
(0, 22, 'UPS Express Early A.M.®'), 
(0, 23, 'UPS Express Saver®'), 
(0, 24, 'UPS Expedited®'), 
(0, 25, 'UPS 3 Day Select® (Canada)'), 
(0, 26, 'UPS Worldwide Express Saver™ (Canada)'), 
(0, 27, 'UPS Worldwide Express Plus™ (Canada)'), 
(0, 28, 'UPS Worldwide Express™ (Canada)'), 
(0, 29, 'UPS Second Day Air Intra'), 
(1, 0, 'UPS® Ground'), 
(1, 1, 'UPS 3 Day Select®'), 
(1, 2, 'UPS 2nd Day Air®'), 
(1, 3, 'UPS 2nd Day Air A.M.®'), 
(1, 4, 'UPS Next Day Air®'), 
(1, 5, 'UPS Next Day Air Saver®'), 
(1, 6, 'UPS Next Day Air® Early'), 
(1, 7, 'UPS Worldwide Express®'), 
(1, 8, 'UPS Worldwide Express Plus®'), 
(1, 9, 'UPS Worldwide Expedited®'), 
(1, 10, 'UPS Worldwide Saver®'), 
(1, 11, 'UPS Standard'), 
(1, 12, 'UPS First Class Innovations®'), 
(1, 13, 'UPS Priority Mail Innovations®'), 
(1, 14, 'UPS Expedited Mail Innovations®'), 
(1, 15, 'UPS Economy Mail Innovations®'), 
(1, 16, 'UPS Priority Mail Innovations®'), 
(1, 17, 'UPS SurePost® Less than 1 LB'), 
(1, 18, 'UPS SurePost® 1 LB or Greater'), 
(1, 19, 'UPS SurePost® Bound Printed Matter'), 
(1, 20, 'UPS SurePost® Media'), 
(1, 21, 'UPS Express®'), 
(1, 22, 'UPS Express Early A.M.®'), 
(1, 23, 'UPS Express Saver®'), 
(1, 24, 'UPS Expedited®'), 
(1, 25, 'UPS 3 Day Select® (Canada)'), 
(1, 26, 'UPS Worldwide Express Saver™ (Canada)'), 
(1, 27, 'UPS Worldwide Express Plus™ (Canada)'), 
(1, 28, 'UPS Worldwide Express™ (Canada)'), 
(1, 29, 'UPS Second Day Air Intra'), 
(2, 0, 'Priority'), 
(2, 1, 'First Class'), 
(2, 2, 'Priority Mail Express'), 
(2, 3, 'Media Mail'), 
(2, 4, 'Library Mail'), 
(2, 5, 'Standard Post'), 
(2, 6, 'Bound Printed Matter'), 
(2, 7, 'Global Express Guaranteed'), 
(2, 8, 'Global Express Guaranteed Non-Document'), 
(2, 9, 'International First'), 
(2, 10, 'International Express'), 
(2, 11, 'International Priority'), 
(2, 12, 'Express Mail (Premium)'), 
(2, 13, 'Parcel Select'), 
(2, 14, 'Critical Mail'), 
(2, 15, 'Pay-on-Use Return'), 
(2, 100, 'DHL SM Parcel Expedited'), 
(2, 101, 'DHL SM Parcel Ground'), 
(2, 102, 'DHL SM Parcel Plus Expedited'), 
(2, 103, 'DHL SM Parcel Plus Ground'), 
(2, 104, 'DHL SM BPM Expedited'), 
(2, 105, 'DHL SM BPM Ground'), 
(2, 106, 'DHL SM Catalog Expedited'), 
(2, 107, 'DHL SM Catalog Ground'), 
(2, 108, 'DHL SM Media Mail Ground'), 
(2, 109, 'DHL SM Marketing Ground'), 
(2, 110, 'DHL SM Marketing Expedited'), 
(2, 200, 'Consolidator Label'), 
(2, 201, 'Consolidator (International)'), 
(2, 202, 'Consolidator (IPA)'), 
(2, 203, 'Consolidator (ISAL)'), 
(2, 204, 'Commercial ePacket'), 
(2, 210, 'Asendia IPA'), 
(2, 211, 'Asendia ISAL'), 
(2, 212, 'Asendia ePacket'), 
(2, 213, 'Asendia Generic'), 
(2, 214, 'DHL Packet IPA'), 
(2, 215, 'DHL Packet ISAL'), 
(2, 216, 'Globegistics IPA'), 
(2, 217, 'Globegistics ISAL'), 
(2, 218, 'Globegistics ePacket'), 
(2, 219, 'Globegistics Generic'), 
(2, 220, 'International Bonded Couriers IPA'), 
(2, 221, 'International Bonded Couriers ISAL'), 
(2, 222, 'International Bonded Couriers ePacket'), 
(2, 223, 'RRD IPA'), 
(2, 224, 'RRD ISAL'), 
(2, 225, 'RRD EPS (ePacket Service)'), 
(2, 226, 'RRD Generic'), 
(2, 227, 'GlobalPost Economy Intl'), 
(2, 228, 'GlobalPost Standard Intl'), 
(2, 229, 'GlobalPost SmartSaver Economy Intl'), 
(2, 230, 'GlobalPost SmartSaver Standard Intl'), 
(2, 231, 'GlobalPost Plus'), 
(2, 232, 'GlobalPost Plus SmartSaver'), 
(2, 233, 'GlobalPost Parcel Select SmartSaver'), 
(4, 0, 'Priority'), 
(4, 1, 'First Class'), 
(4, 2, 'Priority Mail Express'), 
(4, 3, 'Media Mail'), 
(4, 4, 'Library Mail'), 
(4, 5, 'Standard Post'), 
(4, 6, 'Bound Printed Matter'), 
(4, 7, 'Global Express Guaranteed'), 
(4, 8, 'Global Express Guaranteed Non-Document'), 
(4, 9, 'International First'), 
(4, 10, 'International Express'), 
(4, 11, 'International Priority'), 
(4, 12, 'Express Mail (Premium)'), 
(4, 13, 'Parcel Select'), 
(4, 14, 'Critical Mail'), 
(4, 15, 'Pay-on-Use Return'), 
(4, 100, 'DHL SM Parcel Expedited'), 
(4, 101, 'DHL SM Parcel Ground'), 
(4, 102, 'DHL SM Parcel Plus Expedited'), 
(4, 103, 'DHL SM Parcel Plus Ground'), 
(4, 104, 'DHL SM BPM Expedited'), 
(4, 105, 'DHL SM BPM Ground'), 
(4, 106, 'DHL SM Catalog Expedited'), 
(4, 107, 'DHL SM Catalog Ground'), 
(4, 108, 'DHL SM Media Mail Ground'), 
(4, 109, 'DHL SM Marketing Ground'), 
(4, 110, 'DHL SM Marketing Expedited'), 
(4, 200, 'Consolidator Label'), 
(4, 201, 'Consolidator (International)'), 
(4, 202, 'Consolidator (IPA)'), 
(4, 203, 'Consolidator (ISAL)'), 
(4, 204, 'Commercial ePacket'), 
(4, 210, 'Asendia IPA'), 
(4, 211, 'Asendia ISAL'), 
(4, 212, 'Asendia ePacket'), 
(4, 213, 'Asendia Generic'), 
(4, 214, 'DHL Packet IPA'), 
(4, 215, 'DHL Packet ISAL'), 
(4, 216, 'Globegistics IPA'), 
(4, 217, 'Globegistics ISAL'), 
(4, 218, 'Globegistics ePacket'), 
(4, 219, 'Globegistics Generic'), 
(4, 220, 'International Bonded Couriers IPA'), 
(4, 221, 'International Bonded Couriers ISAL'), 
(4, 222, 'International Bonded Couriers ePacket'), 
(4, 223, 'RRD IPA'), 
(4, 224, 'RRD ISAL'), 
(4, 225, 'RRD EPS (ePacket Service)'), 
(4, 226, 'RRD Generic'), 
(4, 227, 'GlobalPost Economy Intl'), 
(4, 228, 'GlobalPost Standard Intl'), 
(4, 229, 'GlobalPost SmartSaver Economy Intl'), 
(4, 230, 'GlobalPost SmartSaver Standard Intl'), 
(4, 231, 'GlobalPost Plus'), 
(4, 232, 'GlobalPost Plus SmartSaver'), 
(4, 233, 'GlobalPost Parcel Select SmartSaver'), 
(6, 0, 'FedEx Priority Overnight®'), 
(6, 1, 'FedEx Standard Overnight®'), 
(6, 2, 'FedEx First Overnight®'), 
(6, 3, 'FedEx 2Day®'), 
(6, 4, 'FedEx Express Saver®'), 
(6, 5, 'FedEx International Priority®'), 
(6, 6, 'FedEx International Economy®'), 
(6, 7, 'FedEx International First®'), 
(6, 8, 'FedEx 1Day® Freight'), 
(6, 9, 'FedEx 2Day® Freight'), 
(6, 10, 'FedEx 3Day® Freight'), 
(6, 11, 'FedEx Ground®'), 
(6, 12, 'FedEx Home Delivery®'), 
(6, 13, 'FedEx International Priority® Freight'), 
(6, 14, 'FedEx International Economy® Freight'), 
(6, 15, 'FedEx SmartPost®'), 
(6, 17, 'FedEx Europe First International Priority®'), 
(6, 18, 'FedEx 2Day® A.M.'), 
(6, 19, 'FedEx First Overnight® Freight'), 
(6, 20, 'FedEx One Rate® (First Overnight)'), 
(6, 21, 'FedEx One Rate® (Priority Overnight)'), 
(6, 22, 'FedEx One Rate® (Standard Overnight)'), 
(6, 23, 'FedEx One Rate® (2Day)'), 
(6, 24, 'FedEx One Rate® (2Day A.M.)'), 
(6, 25, 'FedEx One Rate® (Express Saver)'), 
(6, 26, 'FedEx Economy'), 
(6, 27, 'FedEx FIMS Mailview'), 
(6, 28, 'FedEx International Ground®'), 
(6, 29, 'FedEx Next Day Afternoon'), 
(6, 30, 'FedEx Next Day Early Morning'), 
(6, 31, 'FedEx Next Day Mid Morning'), 
(6, 32, 'FedEx Next Day End Of Day'), 
(6, 33, 'FedEx Distance Deferred'), 
(6, 34, 'FedEx Next Day Freight'), 
(6, 35, 'FedEx FIMS Mailview Lite'), 
(6, 36, 'FedEx FIMS Standard'), 
(6, 37, 'FedEx FIMS Premium'), 
(6, 38, 'FedEx International Priority® Express'), 
(6, 39, 'FedEx Freight® Economy'), 
(6, 40, 'FedEx Freight® Priority'), 
(9, 0, 'Priority'), 
(9, 1, 'First Class'), 
(9, 2, 'Priority Mail Express'), 
(9, 3, 'Media Mail'), 
(9, 4, 'Library Mail'), 
(9, 5, 'Standard Post'), 
(9, 6, 'Bound Printed Matter'), 
(9, 7, 'Global Express Guaranteed'), 
(9, 8, 'Global Express Guaranteed Non-Document'), 
(9, 9, 'International First'), 
(9, 10, 'International Express'), 
(9, 11, 'International Priority'), 
(9, 12, 'Express Mail (Premium)'), 
(9, 13, 'Parcel Select'), 
(9, 14, 'Critical Mail'), 
(9, 15, 'Pay-on-Use Return'), 
(9, 100, 'DHL SM Parcel Expedited'), 
(9, 101, 'DHL SM Parcel Ground'), 
(9, 102, 'DHL SM Parcel Plus Expedited'), 
(9, 103, 'DHL SM Parcel Plus Ground'), 
(9, 104, 'DHL SM BPM Expedited'), 
(9, 105, 'DHL SM BPM Ground'), 
(9, 106, 'DHL SM Catalog Expedited'), 
(9, 107, 'DHL SM Catalog Ground'), 
(9, 108, 'DHL SM Media Mail Ground'), 
(9, 109, 'DHL SM Marketing Ground'), 
(9, 110, 'DHL SM Marketing Expedited'), 
(9, 200, 'Consolidator Label'), 
(9, 201, 'Consolidator (International)'), 
(9, 202, 'Consolidator (IPA)'), 
(9, 203, 'Consolidator (ISAL)'), 
(9, 204, 'Commercial ePacket'), 
(9, 210, 'Asendia IPA'), 
(9, 211, 'Asendia ISAL'), 
(9, 212, 'Asendia ePacket'), 
(9, 213, 'Asendia Generic'), 
(9, 214, 'DHL Packet IPA'), 
(9, 215, 'DHL Packet ISAL'), 
(9, 216, 'Globegistics IPA'), 
(9, 217, 'Globegistics ISAL'), 
(9, 218, 'Globegistics ePacket'), 
(9, 219, 'Globegistics Generic'), 
(9, 220, 'International Bonded Couriers IPA'), 
(9, 221, 'International Bonded Couriers ISAL'), 
(9, 222, 'International Bonded Couriers ePacket'), 
(9, 223, 'RRD IPA'), 
(9, 224, 'RRD ISAL'), 
(9, 225, 'RRD EPS (ePacket Service)'), 
(9, 226, 'RRD Generic'), 
(9, 227, 'GlobalPost Economy Intl'), 
(9, 228, 'GlobalPost Standard Intl'), 
(9, 229, 'GlobalPost SmartSaver Economy Intl'), 
(9, 230, 'GlobalPost SmartSaver Standard Intl'), 
(9, 231, 'GlobalPost Plus'), 
(9, 232, 'GlobalPost Plus SmartSaver'), 
(9, 233, 'GlobalPost Parcel Select SmartSaver'), 
(11, 0, 'Not Available'), 
(11, 1, 'Sunrise'), 
(11, 2, 'Sunrise Gold'), 
(11, 3, 'OnTrac Ground'), 
(11, 4, 'Palletized Freight'), 
(12, 0, 'Immediate'), 
(12, 1, 'Preferred'), 
(12, 2, 'Saver'), 
(12, 3, 'Saver Deferred'), 
(13, 0, 'Priority'), 
(13, 1, 'First Class'), 
(13, 2, 'Priority Mail Express'), 
(13, 3, 'Media Mail'), 
(13, 4, 'Library Mail'), 
(13, 5, 'Standard Post'), 
(13, 6, 'Bound Printed Matter'), 
(13, 7, 'Global Express Guaranteed'), 
(13, 8, 'Global Express Guaranteed Non-Document'), 
(13, 9, 'International First'), 
(13, 10, 'International Express'), 
(13, 11, 'International Priority'), 
(13, 12, 'Express Mail (Premium)'), 
(13, 13, 'Parcel Select'), 
(13, 14, 'Critical Mail'), 
(13, 15, 'Pay-on-Use Return'), 
(13, 100, 'DHL SM Parcel Expedited'), 
(13, 101, 'DHL SM Parcel Ground'), 
(13, 102, 'DHL SM Parcel Plus Expedited'), 
(13, 103, 'DHL SM Parcel Plus Ground'), 
(13, 104, 'DHL SM BPM Expedited'), 
(13, 105, 'DHL SM BPM Ground'), 
(13, 106, 'DHL SM Catalog Expedited'), 
(13, 107, 'DHL SM Catalog Ground'), 
(13, 108, 'DHL SM Media Mail Ground'), 
(13, 109, 'DHL SM Marketing Ground'), 
(13, 110, 'DHL SM Marketing Expedited'), 
(13, 200, 'Consolidator Label'), 
(13, 201, 'Consolidator (International)'), 
(13, 202, 'Consolidator (IPA)'), 
(13, 203, 'Consolidator (ISAL)'), 
(13, 204, 'Commercial ePacket'), 
(13, 210, 'Asendia IPA'), 
(13, 211, 'Asendia ISAL'), 
(13, 212, 'Asendia ePacket'), 
(13, 213, 'Asendia Generic'), 
(13, 214, 'DHL Packet IPA'), 
(13, 215, 'DHL Packet ISAL'), 
(13, 216, 'Globegistics IPA'), 
(13, 217, 'Globegistics ISAL'), 
(13, 218, 'Globegistics ePacket'), 
(13, 219, 'Globegistics Generic'), 
(13, 220, 'International Bonded Couriers IPA'), 
(13, 221, 'International Bonded Couriers ISAL'), 
(13, 222, 'International Bonded Couriers ePacket'), 
(13, 223, 'RRD IPA'), 
(13, 224, 'RRD ISAL'), 
(13, 225, 'RRD EPS (ePacket Service)'), 
(13, 226, 'RRD Generic'), 
(13, 227, 'GlobalPost Economy Intl'), 
(13, 228, 'GlobalPost Standard Intl'), 
(13, 229, 'GlobalPost SmartSaver Economy Intl'), 
(13, 230, 'GlobalPost SmartSaver Standard Intl'), 
(13, 231, 'GlobalPost Plus'), 
(13, 232, 'GlobalPost Plus SmartSaver'), 
(13, 233, 'GlobalPost Parcel Select SmartSaver'), 
(15, 0, 'Priority'), 
(15, 1, 'First Class'), 
(15, 2, 'Priority Mail Express'), 
(15, 3, 'Media Mail'), 
(15, 4, 'Library Mail'), 
(15, 5, 'Standard Post'), 
(15, 6, 'Bound Printed Matter'), 
(15, 7, 'Global Express Guaranteed'), 
(15, 8, 'Global Express Guaranteed Non-Document'), 
(15, 9, 'International First'), 
(15, 10, 'International Express'), 
(15, 11, 'International Priority'), 
(15, 12, 'Express Mail (Premium)'), 
(15, 13, 'Parcel Select'), 
(15, 14, 'Critical Mail'), 
(15, 15, 'Pay-on-Use Return'), 
(15, 100, 'DHL SM Parcel Expedited'), 
(15, 101, 'DHL SM Parcel Ground'), 
(15, 102, 'DHL SM Parcel Plus Expedited'), 
(15, 103, 'DHL SM Parcel Plus Ground'), 
(15, 104, 'DHL SM BPM Expedited'), 
(15, 105, 'DHL SM BPM Ground'), 
(15, 106, 'DHL SM Catalog Expedited'), 
(15, 107, 'DHL SM Catalog Ground'), 
(15, 108, 'DHL SM Media Mail Ground'), 
(15, 109, 'DHL SM Marketing Ground'), 
(15, 110, 'DHL SM Marketing Expedited'), 
(15, 200, 'Consolidator Label'), 
(15, 201, 'Consolidator (International)'), 
(15, 202, 'Consolidator (IPA)'), 
(15, 203, 'Consolidator (ISAL)'), 
(15, 204, 'Commercial ePacket'), 
(15, 210, 'Asendia IPA'), 
(15, 211, 'Asendia ISAL'), 
(15, 212, 'Asendia ePacket'), 
(15, 213, 'Asendia Generic'), 
(15, 214, 'DHL Packet IPA'), 
(15, 215, 'DHL Packet ISAL'), 
(15, 216, 'Globegistics IPA'), 
(15, 217, 'Globegistics ISAL'), 
(15, 218, 'Globegistics ePacket'), 
(15, 219, 'Globegistics Generic'), 
(15, 220, 'International Bonded Couriers IPA'), 
(15, 221, 'International Bonded Couriers ISAL'), 
(15, 222, 'International Bonded Couriers ePacket'), 
(15, 223, 'RRD IPA'), 
(15, 224, 'RRD ISAL'), 
(15, 225, 'RRD EPS (ePacket Service)'), 
(15, 226, 'RRD Generic'), 
(15, 227, 'GlobalPost Economy Intl'), 
(15, 228, 'GlobalPost Standard Intl'), 
(15, 229, 'GlobalPost SmartSaver Economy Intl'), 
(15, 230, 'GlobalPost SmartSaver Standard Intl'), 
(15, 231, 'GlobalPost Plus'), 
(15, 232, 'GlobalPost Plus SmartSaver'), 
(15, 233, 'GlobalPost Parcel Select SmartSaver'), 
(17, 0, 'Express Worldwide'), 
(17, 1, 'Express Envelope'), 
(17, 2, 'Express Worldwide Documents'), 
(18, 0, 'Asendia Priority Tracked'), 
(18, 1, 'Asendia International Express'), 
(18, 2, 'Asendia IPA'), 
(18, 3, 'Asendia ISAL'), 
(18, 4, 'Asendia PMI'), 
(18, 5, 'Asendia PMEI'), 
(18, 6, 'Asendia ePacket'), 
(18, 7, 'Asendia Other'), 
(19, 1, 'Ground')
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PackagingType]') AND type IN (N'U'))
DROP TABLE PackagingType
GO
CREATE TABLE PackagingType (ShipmentTypeID int NOT NULL, PackagingTypeID int NOT NULL, Name varchar(50), CONSTRAINT PK_PackagingType PRIMARY KEY (ShipmentTypeID, PackagingTypeID))
GO
INSERT INTO PackagingType (ShipmentTypeID, PackagingTypeID, Name) VALUES
(0, 0, 'Your Packaging'), 
(0, 1, 'UPS Letter'), 
(0, 2, 'UPS Tube'), 
(0, 3, 'UPS Express® Pak'), 
(0, 4, 'UPS Express® Box - Small'), 
(0, 5, 'UPS Express® Box - Medium'), 
(0, 6, 'UPS Express® Box - Large'), 
(0, 7, 'UPS 25 KG Box®'), 
(0, 8, 'UPS 10 KG Box®'), 
(0, 9, 'First Class'), 
(0, 10, 'Priority'), 
(0, 11, 'BPM Flats'), 
(0, 12, 'BPM Parcels'), 
(0, 13, 'Irregulars'), 
(0, 14, 'Machinables'), 
(0, 15, 'Media Mail'), 
(0, 16, 'Parcel Post'), 
(0, 17, 'Standard Flats'), 
(0, 18, 'Flats'), 
(0, 19, 'BPM'), 
(0, 20, 'Parcels'), 
(0, 21, 'UPS Express Box'), 
(0, 22, 'UPS Express Envelope'), 
(1, 0, 'Your Packaging'), 
(1, 1, 'UPS Letter'), 
(1, 2, 'UPS Tube'), 
(1, 3, 'UPS Express® Pak'), 
(1, 4, 'UPS Express® Box - Small'), 
(1, 5, 'UPS Express® Box - Medium'), 
(1, 6, 'UPS Express® Box - Large'), 
(1, 7, 'UPS 25 KG Box®'), 
(1, 8, 'UPS 10 KG Box®'), 
(1, 9, 'First Class'), 
(1, 10, 'Priority'), 
(1, 11, 'BPM Flats'), 
(1, 12, 'BPM Parcels'), 
(1, 13, 'Irregulars'), 
(1, 14, 'Machinables'), 
(1, 15, 'Media Mail'), 
(1, 16, 'Parcel Post'), 
(1, 17, 'Standard Flats'), 
(1, 18, 'Flats'), 
(1, 19, 'BPM'), 
(1, 20, 'Parcels'), 
(1, 21, 'UPS Express Box'), 
(1, 22, 'UPS Express Envelope'), 
(2, 0, 'Package'), 
(2, 1, 'Envelope'), 
(2, 2, 'Large Envelope'), 
(2, 3, 'Flat Rate Envelope'), 
(2, 4, 'Flat Rate Medium Box'), 
(2, 5, 'Flat Rate Large Box'), 
(2, 6, 'Flat Rate Small Box'), 
(2, 7, 'Flat Rate Padded Envelope'), 
(2, 8, 'Regional Rate Box A'), 
(2, 9, 'Regional Rate Box B'), 
(2, 10, 'Flat Rate Legal Envelope'), 
(2, 11, 'Regional Rate Box C'), 
(2, 12, 'Cubic'), 
(2, 13, 'Cubic Soft Pack'), 
(4, 0, 'Package'), 
(4, 1, 'Envelope'), 
(4, 2, 'Large Envelope'), 
(4, 3, 'Flat Rate Envelope'), 
(4, 4, 'Flat Rate Medium Box'), 
(4, 5, 'Flat Rate Large Box'), 
(4, 6, 'Flat Rate Small Box'), 
(4, 7, 'Flat Rate Padded Envelope'), 
(4, 8, 'Regional Rate Box A'), 
(4, 9, 'Regional Rate Box B'), 
(4, 10, 'Flat Rate Legal Envelope'), 
(4, 11, 'Regional Rate Box C'), 
(4, 12, 'Cubic'), 
(4, 13, 'Cubic Soft Pack'), 
(6, 0, 'FedEx® Envelope'), 
(6, 1, 'FedEx® Pak'), 
(6, 2, 'FedEx® Box'), 
(6, 3, 'FedEx® Tube'), 
(6, 4, 'FedEx® 10kg Box'), 
(6, 5, 'FedEx® 25kg Box'), 
(6, 6, 'Your Packaging'), 
(6, 7, 'SmartPost® Media Mail'), 
(6, 8, 'SmartPost® Parcel Select'), 
(6, 9, 'SmartPost® Presorted  BPM'), 
(6, 10, 'SmartPost® Presorted Standard'), 
(6, 11, 'FedEx® Small Box'), 
(6, 12, 'FedEx® Medium Box'), 
(6, 13, 'FedEx® Large Box'), 
(6, 14, 'FedEx® Extra Large Box'), 
(9, 0, 'Package'), 
(9, 1, 'Envelope'), 
(9, 2, 'Large Envelope'), 
(9, 3, 'Flat Rate Envelope'), 
(9, 4, 'Flat Rate Medium Box'), 
(9, 5, 'Flat Rate Large Box'), 
(9, 6, 'Flat Rate Small Box'), 
(9, 7, 'Flat Rate Padded Envelope'), 
(9, 8, 'Regional Rate Box A'), 
(9, 9, 'Regional Rate Box B'), 
(9, 10, 'Flat Rate Legal Envelope'), 
(9, 11, 'Regional Rate Box C'), 
(9, 12, 'Cubic'), 
(9, 13, 'Cubic Soft Pack'), 
(11, 0, 'Package'), 
(11, 1, 'Letter'), 
(13, 0, 'Package'), 
(13, 1, 'Envelope'), 
(13, 2, 'Large Envelope'), 
(13, 3, 'Flat Rate Envelope'), 
(13, 4, 'Flat Rate Medium Box'), 
(13, 5, 'Flat Rate Large Box'), 
(13, 6, 'Flat Rate Small Box'), 
(13, 7, 'Flat Rate Padded Envelope'), 
(13, 8, 'Regional Rate Box A'), 
(13, 9, 'Regional Rate Box B'), 
(13, 10, 'Flat Rate Legal Envelope'), 
(13, 11, 'Regional Rate Box C'), 
(13, 12, 'Cubic'), 
(13, 13, 'Cubic Soft Pack'), 
(15, 0, 'Package'), 
(15, 1, 'Envelope'), 
(15, 2, 'Large Envelope'), 
(15, 3, 'Flat Rate Envelope'), 
(15, 4, 'Flat Rate Medium Box'), 
(15, 5, 'Flat Rate Large Box'), 
(15, 6, 'Flat Rate Small Box'), 
(15, 7, 'Flat Rate Padded Envelope'), 
(15, 8, 'Regional Rate Box A'), 
(15, 9, 'Regional Rate Box B'), 
(15, 10, 'Flat Rate Legal Envelope'), 
(15, 11, 'Regional Rate Box C'), 
(15, 12, 'Cubic'), 
(15, 13, 'Cubic Soft Pack')
