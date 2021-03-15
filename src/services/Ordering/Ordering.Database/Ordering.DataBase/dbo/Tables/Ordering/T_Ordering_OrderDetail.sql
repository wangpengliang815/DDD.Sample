CREATE TABLE [dbo].[T_Ordering_OrderDetail]
(
	[Id] NVARCHAR(100) NOT NULL , 
    [OrderId]  NVARCHAR(100) NULL, 
    [GoodsId] NVARCHAR(100) NULL, 
    [GoodsName] NVARCHAR(100) NULL,
    [Number] INT NULL, 
    [Creator]                  NVARCHAR (100)  NULL,
    [Created]                  DATETIME    NULL,
    [Editor]                   NVARCHAR (100)  NULL,
    [Edited]                   DATETIME    NULL, 
    [IsDeleted]                BIT NULL, 
    CONSTRAINT [PK_T_Ordering_OrderDetail] PRIMARY KEY ([Id]), 
)
