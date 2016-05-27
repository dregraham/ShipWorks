UPDATE [Template] 
SET Xsl = REPLACE(Xsl,'http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum={TrackingNumber}','https://tools.usps.com/go/TrackConfirmAction.action?tLabels={TrackingNumber}')
GO