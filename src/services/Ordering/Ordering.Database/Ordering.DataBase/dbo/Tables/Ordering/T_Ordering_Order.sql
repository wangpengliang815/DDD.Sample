CREATE TABLE [dbo].[T_Ordering_Order]
(
	[Id] NVARCHAR(100) NOT NULL , 
    [ConsigneeName] NVARCHAR(100) NULL, 
    [ConsigneePhone] NVARCHAR(100) NULL,
    [TotalPrice] DECIMAL NULL, 
    [Status] INT   NULL,
    [Creator]                  NVARCHAR (100)  NULL,
    [Created]                  DATETIME    NULL,
    [Editor]                   NVARCHAR (100)  NULL,
    [Edited]                   DATETIME    NULL, 
    [IsDeleted]                BIT NULL, 
    CONSTRAINT [PK_T_Ordering_Order] PRIMARY KEY ([Id]), 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收货人名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Ordering_Order',
    @level2type = N'COLUMN',
    @level2name = N'ConsigneeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收货人手机号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Ordering_Order',
    @level2type = N'COLUMN',
    @level2name = N'ConsigneePhone'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'总金额',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Ordering_Order',
    @level2type = N'COLUMN',
    @level2name = N'TotalPrice'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'状态',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Ordering_Order',
    @level2type = N'COLUMN',
    @level2name = N'Status'