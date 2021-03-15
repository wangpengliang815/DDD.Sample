CREATE TABLE [dbo].[T_Store_Goods]
(
	[Id] NVARCHAR(100) NOT NULL , 
    [GoodsId] NVARCHAR(100) NULL, 
    [GoodsName] NVARCHAR(100) NULL,
    [SurplusNumber] INT NULL, 
    [Creator]                  NVARCHAR (100)  NULL,
    [Created]                  DATETIME    NULL,
    [Editor]                   NVARCHAR (100)  NULL,
    [Edited]                   DATETIME    NULL, 
    [IsDeleted]                BIT NULL, 
    CONSTRAINT [PK_T_Store_Goods] PRIMARY KEY ([Id]), 
)

GO