CREATE TABLE [dbo].[AuditChangeDetail] (
    [AuditChangeDetailID] BIGINT         IDENTITY (1047, 1000) NOT NULL,
    [AuditChangeID]       BIGINT         NOT NULL,
    [AuditID]             BIGINT         NOT NULL,
    [DisplayName]         VARCHAR (50)   NOT NULL,
    [DisplayFormat]       TINYINT        NOT NULL,
    [DataType]            TINYINT        NOT NULL,
    [TextOld]             NVARCHAR (MAX) NULL,
    [TextNew]             NVARCHAR (MAX) NULL,
    [VariantOld]          SQL_VARIANT    NULL,
    [VariantNew]          SQL_VARIANT    NULL,
    CONSTRAINT [PK_AuditChangeDetail] PRIMARY KEY CLUSTERED ([AuditChangeDetailID] ASC),
    CONSTRAINT [FK_AuditChangeDetail_AuditChange] FOREIGN KEY ([AuditChangeID]) REFERENCES [dbo].[AuditChange] ([AuditChangeID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail]
    ON [dbo].[AuditChangeDetail]([AuditChangeID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail_VariantNew]
    ON [dbo].[AuditChangeDetail]([VariantNew] ASC)
    INCLUDE([AuditID]);

