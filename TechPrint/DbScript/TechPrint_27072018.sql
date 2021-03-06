USE [TechPrint]
GO
/****** Object:  StoredProcedure [dbo].[USP_SEARCH_QUOTATION_LIST]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_SEARCH_QUOTATION_LIST]
GO
/****** Object:  StoredProcedure [dbo].[USP_SEARCH_PAYMENT_LIST]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_SEARCH_PAYMENT_LIST]
GO
/****** Object:  StoredProcedure [dbo].[USP_PAYMENT]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_PAYMENT]
GO
/****** Object:  StoredProcedure [dbo].[USP_MANAGE_PRINT_RATE]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_MANAGE_PRINT_RATE]
GO
/****** Object:  StoredProcedure [dbo].[USP_MANAGE_JOB_SHEET]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_MANAGE_JOB_SHEET]
GO
/****** Object:  StoredProcedure [dbo].[USP_DATE_WISE_PAYMENT_SUMMERY]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_DATE_WISE_PAYMENT_SUMMERY]
GO
/****** Object:  StoredProcedure [dbo].[USP_DATE_WISE_JOB_SUMMERY]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_DATE_WISE_JOB_SUMMERY]
GO
/****** Object:  StoredProcedure [dbo].[USP_BIND_MASTER_DATA]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP PROCEDURE [dbo].[USP_BIND_MASTER_DATA]
GO
/****** Object:  UserDefinedTableType [dbo].[QUOTATION_DETAIL]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP TYPE [dbo].[QUOTATION_DETAIL]
GO
/****** Object:  UserDefinedTableType [dbo].[PRINT_RATE]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP TYPE [dbo].[PRINT_RATE]
GO
/****** Object:  UserDefinedTableType [dbo].[PAPER_DETAIL]    Script Date: 7/27/2018 1:46:38 AM ******/
DROP TYPE [dbo].[PAPER_DETAIL]
GO
/****** Object:  UserDefinedTableType [dbo].[PAPER_DETAIL]    Script Date: 7/27/2018 1:46:38 AM ******/
CREATE TYPE [dbo].[PAPER_DETAIL] AS TABLE(
	[PrintComponentID] [int] NULL DEFAULT ((0)),
	[NoofSheet] [int] NULL DEFAULT ((0)),
	[PaperSize] [int] NULL DEFAULT ((0)),
	[PaperLength] [decimal](18, 2) NULL DEFAULT ((0)),
	[Paperwidth] [decimal](18, 2) NULL DEFAULT ((0)),
	[GSM] [int] NULL DEFAULT ((0)),
	[GSMValue] [decimal](18, 2) NULL DEFAULT ((0)),
	[PaperType] [int] NULL DEFAULT ((0)),
	[PaperManufature] [int] NULL DEFAULT ((0)),
	[PaperRate] [decimal](18, 2) NULL DEFAULT ((0)),
	[Amount] [decimal](18, 2) NULL DEFAULT ((0)),
	[PaperDetailGST] [decimal](18, 2) NULL DEFAULT ((0))
)
GO
/****** Object:  UserDefinedTableType [dbo].[PRINT_RATE]    Script Date: 7/27/2018 1:46:38 AM ******/
CREATE TYPE [dbo].[PRINT_RATE] AS TABLE(
	[PlateID] [int] NULL,
	[ColorID] [int] NULL,
	[PlateCost1K] [decimal](18, 2) NULL,
	[ColorCost1K] [decimal](18, 2) NULL,
	[PlateCost10K] [decimal](18, 2) NULL,
	[ColorCost10K] [decimal](18, 2) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[QUOTATION_DETAIL]    Script Date: 7/27/2018 1:46:38 AM ******/
CREATE TYPE [dbo].[QUOTATION_DETAIL] AS TABLE(
	[PrintComponentID] [int] NULL DEFAULT ((0)),
	[NoOfSheet] [int] NULL DEFAULT ((0)),
	[GSMID] [int] NULL DEFAULT ((0)),
	[GSMValue] [decimal](18, 2) NULL DEFAULT ((0)),
	[PaperSizeID] [int] NULL DEFAULT ((0)),
	[PaperLength] [decimal](18, 2) NULL DEFAULT ((0)),
	[PaperWidth] [decimal](18, 2) NULL DEFAULT ((0)),
	[PaperTypeID] [int] NULL DEFAULT ((0)),
	[PaperManufaturerID] [int] NULL DEFAULT ((0)),
	[NoOfSheetPrintDetail] [int] NULL DEFAULT ((0)),
	[PaperSizeIDPrintDetail] [int] NULL DEFAULT ((0)),
	[Impression] [decimal](18, 2) NULL DEFAULT ((0)),
	[CuttingID] [int] NULL DEFAULT ((0)),
	[NoofCuttingPices] [int] NULL DEFAULT ((0)),
	[FinishSize] [decimal](18, 2) NULL DEFAULT ((0)),
	[SideID] [int] NULL DEFAULT ((0)),
	[ColourID] [int] NULL DEFAULT ((0)),
	[PlateTypeID] [int] NULL DEFAULT ((0)),
	[MachineID] [int] NULL DEFAULT ((0)),
	[FabricationItemID] [int] NULL DEFAULT ((0)),
	[FabricationItemSize] [varchar](50) NULL,
	[PackagingItemID] [int] NULL DEFAULT ((0)),
	[LabourCharge] [decimal](18, 2) NULL DEFAULT ((0)),
	[TransportCharge] [decimal](18, 2) NULL DEFAULT ((0)),
	[TransportUnitID] [int] NULL DEFAULT ((0)),
	[OperatorID] [int] NULL DEFAULT ((0)),
	[Narration] [varchar](500) NULL,
	[LineQuantity] [decimal](18, 2) NULL DEFAULT ((0)),
	[LineRate] [decimal](18, 2) NULL DEFAULT ((0)),
	[LineAmount] [decimal](18, 2) NULL DEFAULT ((0)),
	[LineGST] [decimal](18, 2) NULL DEFAULT ((0))
)
GO
/****** Object:  StoredProcedure [dbo].[USP_BIND_MASTER_DATA]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec USP_BIND_MASTER_DATA 1
CREATE PROCEDURE [dbo].[USP_BIND_MASTER_DATA]
	@MasterTypeId INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @MasterTypeId > 0
		BEGIN
			SELECT MP.PrintingParameterID,MT.PrintingParameterTypeName,UPPER(MP.PrintingParameterName) AS PrintingParameterName,MP.PrintingParameterDescription
				,MP.ParameterValue1,MP.ParameterValue2,MP.ParameterValue3,MP.SellPrice
					FROM PrintingParameter MP INNER JOIN PrintingParameterType MT ON MP.PrintingParameterTypeID = MT.PrintingParameterTypeID
						WHERE MP.RecMode <> 'D' AND MT.PrintingParameterTypeID = @MasterTypeId
							ORDER BY MP.PrintingParameterName
		END
    ELSE
		BEGIN
			SELECT MP.PrintingParameterID,MT.PrintingParameterTypeName,UPPER(MP.PrintingParameterName) AS PrintingParameterName,MP.PrintingParameterDescription
				,MP.ParameterValue1,MP.ParameterValue2,MP.ParameterValue3,MP.SellPrice
					FROM PrintingParameter MP INNER JOIN PrintingParameterType MT ON MP.PrintingParameterTypeID = MT.PrintingParameterTypeID
						WHERE MP.RecMode <> 'D'
							ORDER BY MP.PrintingParameterName
		END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DATE_WISE_JOB_SUMMERY]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_DATE_WISE_JOB_SUMMERY '2018-01-01','2018-07-27',0
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DATE_WISE_JOB_SUMMERY]
	@FromDate DATE,
	@ToDate DATE,
	@CustomerID INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		IF @CustomerID > 0
			BEGIN
				SELECT H.QuotationID,H.QuotationNO,CONVERT(VARCHAR,H.QuotationDate,103) AS QuotationDate,C.CustomerName,C.Telephone
					,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName + CHAR(10) + H.ItemDescription FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE H.ItemDescription END AS Item
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR,D.GSMValue) + ',' + CONVERT(VARCHAR,D.NoOfSheet) + ',' + CONVERT(VARCHAR,D.PaperLength) + 'x' + CONVERT(VARCHAR,D.PaperWidth) 
							+ CASE WHEN D.PaperTypeID > 0 THEN (SELECT ', ' + UPPER(T.PrintingParameterName) FROM PrintingParameter T WHERE PrintingParameterID = D.PaperTypeID) ELSE '' END
								+ CASE WHEN D.PaperManufaturerID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PaperManufaturerID) ELSE '' END
									FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 1 FOR XML PATH ('')), 1, 1, ''
					   ) AS PaperDetail
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR,CONVERT(INT,D.Impression))
						+ CASE WHEN D.PaperSizeIDPrintDetail > 0 THEN (SELECT ', ' + UPPER(p.PrintingParameterName) FROM PrintingParameter p WHERE PrintingParameterID = D.PaperSizeIDPrintDetail) ELSE '' END
						+ CASE WHEN D.CuttingID > 0 THEN (SELECT ', ' + UPPER(C.PrintingParameterName) FROM PrintingParameter C WHERE PrintingParameterID = D.CuttingID) ELSE '' END
						+ CASE WHEN D.ColourID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.ColourID) ELSE '' END
						+ CASE WHEN D.PlateTypeID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PlateTypeID) ELSE '' END
						+ CASE WHEN D.MachineID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.MachineID) ELSE '' END 
						FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 2 FOR XML PATH ('')), 1, 1, ''
					   ) AS Printing
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.FabricationItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
						FROM PrintingParameter p WHERE PrintingParameterID = D.FabricationItemID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.FabricationItemSize,''))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
							+ ',' + CONVERT(VARCHAR,ISNULL(D.LineRate,0)) 
							FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 3 FOR XML PATH ('')), 1, 1, ''
					   ) AS FabricationItem
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.PackagingItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
						FROM PrintingParameter p WHERE PrintingParameterID = D.PackagingItemID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0)) FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 4 FOR XML PATH ('')), 1, 1, ''
					   ) AS Packaging
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR(50),ISNULL(D.LabourCharge,0))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.TransportCharge,0))
							+ CASE WHEN D.TransportUnitID > 0 THEN (SELECT ',' + UPPER(p.PrintingParameterName) 
								FROM PrintingParameter p WHERE PrintingParameterID = D.TransportUnitID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0)) FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 5 FOR XML PATH ('')), 1, 1, ''
					   ) AS TransportLabour
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.OperatorID > 0 THEN (SELECT UPPER(p.PrintingParameterName) + ', ' + UPPER(D.Narration) + ', ' + CONVERT(VARCHAR(50),D.LineAmount)
						FROM PrintingParameter p WHERE PrintingParameterID = D.OperatorID) ELSE '' END 
						FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 6 FOR XML PATH ('')), 1, 1, ''
					   ) AS Design
							FROM QuotationJobSheet H 
								INNER JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE H.QuotationDate BETWEEN @FromDate AND @ToDate AND H.CustomerID = @CustomerID AND H.IS_JOB = 1
			END
		ELSE
			BEGIN
				SELECT H.QuotationID,H.QuotationNO,CONVERT(VARCHAR,H.QuotationDate,103) AS QuotationDate,C.CustomerName,C.Telephone
					,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName + CHAR(10) + H.ItemDescription FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE H.ItemDescription END AS Item
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR,D.GSMValue) + ',' + CONVERT(VARCHAR,D.NoOfSheet) + ',' + CONVERT(VARCHAR,D.PaperLength) + 'x' + CONVERT(VARCHAR,D.PaperWidth) 
							+ CASE WHEN D.PaperTypeID > 0 THEN (SELECT ', ' + UPPER(T.PrintingParameterName) FROM PrintingParameter T WHERE PrintingParameterID = D.PaperTypeID) ELSE '' END
								+ CASE WHEN D.PaperManufaturerID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PaperManufaturerID) ELSE '' END
									FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 1 FOR XML PATH ('')), 1, 1, ''
					   ) AS PaperDetail
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR,CONVERT(INT,D.Impression))
						+ CASE WHEN D.PaperSizeIDPrintDetail > 0 THEN (SELECT ', ' + UPPER(p.PrintingParameterName) FROM PrintingParameter p WHERE PrintingParameterID = D.PaperSizeIDPrintDetail) ELSE '' END
						+ CASE WHEN D.CuttingID > 0 THEN (SELECT ', ' + UPPER(C.PrintingParameterName) FROM PrintingParameter C WHERE PrintingParameterID = D.CuttingID) ELSE '' END
						+ CASE WHEN D.ColourID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.ColourID) ELSE '' END
						+ CASE WHEN D.PlateTypeID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PlateTypeID) ELSE '' END
						+ CASE WHEN D.MachineID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.MachineID) ELSE '' END 
						FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 2 FOR XML PATH ('')), 1, 1, ''
					   ) AS Printing
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.FabricationItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
						FROM PrintingParameter p WHERE PrintingParameterID = D.FabricationItemID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.FabricationItemSize,''))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
							+ ',' + CONVERT(VARCHAR,ISNULL(D.LineRate,0)) 
							FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 3 FOR XML PATH ('')), 1, 1, ''
					   ) AS FabricationItem
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.PackagingItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
						FROM PrintingParameter p WHERE PrintingParameterID = D.PackagingItemID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0)) FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 4 FOR XML PATH ('')), 1, 1, ''
					   ) AS Packaging
					,STUFF(
						 (SELECT CHAR(10) + CONVERT(VARCHAR(50),ISNULL(D.LabourCharge,0))
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.TransportCharge,0))
							+ CASE WHEN D.TransportUnitID > 0 THEN (SELECT ',' + UPPER(p.PrintingParameterName) 
								FROM PrintingParameter p WHERE PrintingParameterID = D.TransportUnitID) ELSE '' END
							+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0)) FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 5 FOR XML PATH ('')), 1, 1, ''
					   ) AS TransportLabour
					,STUFF(
						 (SELECT CHAR(10) + CASE WHEN D.OperatorID > 0 THEN (SELECT UPPER(p.PrintingParameterName) + ', ' + UPPER(D.Narration) + ', ' + CONVERT(VARCHAR(50),D.LineAmount)
						FROM PrintingParameter p WHERE PrintingParameterID = D.OperatorID) ELSE '' END 
						FROM QuotationJobSheetDetail D WHERE D.QuotationID = H.QuotationID AND D.PrintComponentID = 6 FOR XML PATH ('')), 1, 1, ''
					   ) AS Design
							FROM QuotationJobSheet H 
								INNER JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE H.QuotationDate BETWEEN @FromDate AND @ToDate AND H.IS_JOB = 1
			END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DATE_WISE_PAYMENT_SUMMERY]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_DATE_WISE_PAYMENT_SUMMERY '2018-01-01','2018-07-27',0
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DATE_WISE_PAYMENT_SUMMERY]
	@FromDate DATE,
	@ToDate DATE,
	@CustomerID INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		IF @CustomerID > 0
			BEGIN
				SELECT P.PaymentID,P.PaymentNumber,CONVERT(VARCHAR,P.PaymentDate,103) AS PaymentDate,C.CustomerName,C.GSTNo,LOWER(C.Email) AS Email,C.Telephone,P.PaidAmount,P.PaymentDetail
					FROM Payment P INNER JOIN Customer C ON P.CustomerID = C.CustomerID
						WHERE P.CustomerID = @CustomerID AND P.PaymentDate BETWEEN @FromDate AND @ToDate
			END
		ELSE
			BEGIN
				SELECT P.PaymentID,P.PaymentNumber,CONVERT(VARCHAR,P.PaymentDate,103) AS PaymentDate,C.CustomerName,C.GSTNo,LOWER(C.Email) AS Email,C.Telephone,P.PaidAmount,P.PaymentDetail
					FROM Payment P INNER JOIN Customer C ON P.CustomerID = C.CustomerID
						WHERE P.PaymentDate BETWEEN @FromDate AND @ToDate
			END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_MANAGE_JOB_SHEET]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_MANAGE_JOB_SHEET]
	@QuotationID BIGINT = 0,
	@QuotationNO VARCHAR(50) = '',
	@QuotationDate date,
	@IS_JOB bit = 0,
	@CustomerID int = 0,
	@ItemID int = 0,
	@ItemDescription varchar(500) = '',
	@TaxID int = 0,
	@GSTPercentage decimal(18, 2) = 0,
	@GSTWOPaper decimal(18 ,2) = 0,
	@CGSTAmount decimal(18, 2) = 0,
	@SGSTAmount decimal(18, 2) = 0,
	@IGSTAmount decimal(18, 2) = 0,
	@TaxableAmount decimal(18, 2) = 0,
	@Discount decimal(18, 2) = 0,
	@TotalBillingAmount decimal(18, 2) = 0,
	@AmountPayable decimal(18, 2) = 0,
	@RoundOff decimal(18, 2) = 0,
	@AmountPaid decimal(18, 2) = 0,
	@Remarks varchar(500) = '',
	@FinYear varchar(50) = '',
	@RequestBy INT = 0,
	@QuotationDetail dbo.[QUOTATION_DETAIL] READONLY,
	@OpMode INT = 0 -- 1. INSERT 2. UPDATE 3. DELETE 4. SELECT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	IF @OpMode = 1
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
					IF @QuotationNO = '' OR @QuotationNO IS NULL
						BEGIN
							SET @QuotationNO = (SELECT 'Q/' + @FinYear + '/' + RIGHT('00000'+ CONVERT(NVARCHAR(10),ISNULL(COUNT(*),0) + 1),5) FROM QuotationJobSheet WHERE FinYear = @FinYear)
						END
					ELSE IF EXISTS(SELECT * FROM QuotationJobSheet WHERE QuotationNO = @QuotationNO)
						BEGIN
							SET @QuotationNO = (SELECT 'Q/' + @FinYear + '/' + RIGHT('00000'+ CONVERT(NVARCHAR(10),ISNULL(COUNT(*),0) + 1),5) FROM QuotationJobSheet WHERE FinYear = @FinYear)
						END


					INSERT INTO QuotationJobSheet([QuotationNO],[QuotationDate],[IS_JOB],[CustomerID],[ItemID],[ItemDescription],[TaxID]
												,[GSTPercentage],[GSTWOPaper],[CGSTAmount],[SGSTAmount],[IGSTAmount],[TaxableAmount],[Discount],[RoundOff],[TotalBillingAmount]
												,[AmountPayable],[OutstandingAmount],[Remarks],[FinYear],[RecMode],[CreatedBy],[CreateDate])
					VALUES(@QuotationNO,@QuotationDate,@IS_JOB,@CustomerID,@ItemID,@ItemDescription,@TaxID,@GSTPercentage,@GSTWOPaper
							,@CGSTAmount,@SGSTAmount,@IGSTAmount,@TaxableAmount,@Discount,@RoundOff,@TotalBillingAmount
							,@AmountPayable,@AmountPayable,@Remarks,@FinYear,'A',@RequestBy,GETDATE())
					
					SET @QuotationID = SCOPE_IDENTITY()

					INSERT INTO QuotationJobSheetDetail([QuotationID],[PrintComponentID],[NoOfSheet],[GSMID],[GSMValue]
													  ,[PaperSizeID],[PaperLength],[PaperWidth],[PaperTypeID],[PaperManufaturerID]
													  ,[NoOfSheetPrintDetail],[PaperSizeIDPrintDetail]
													  ,[Impression],[CuttingID],[NoofCuttingPices]
													  ,[FinishSize],[SideID],[ColourID],[PlateTypeID],[MachineID],[FabricationItemID],[FabricationItemSize]
													  ,[PackagingItemID],[LabourCharge],[TransportCharge],[TransportUnitID],[OperatorID],[Narration]
													  ,[LineQuantity],[LineRate],[LineAmount],[LineGST])

					SELECT @QuotationID,[PrintComponentID],[NoOfSheet],ISNULL([GSMID],0),ISNULL([GSMValue],0)
							,ISNULL([PaperSizeID],0),ISNULL([PaperLength],0),ISNULL([PaperWidth],0),ISNULL([PaperTypeID],0),ISNULL([PaperManufaturerID],0)
							,ISNULL([NoOfSheetPrintDetail],0),ISNULL([PaperSizeIDPrintDetail],0)
							,ISNULL([Impression],0),ISNULL([CuttingID],0),ISNULL([NoofCuttingPices],0)
							,[FinishSize],ISNULL([SideID],0),ISNULL([ColourID],0),[PlateTypeID],[MachineID],[FabricationItemID],[FabricationItemSize]
							,ISNULL([PackagingItemID],0),ISNULL([LabourCharge],0),ISNULL([TransportCharge],0),ISNULL([TransportUnitID],0),ISNULL([OperatorID],0),[Narration]
							,ISNULL([LineQuantity],0),ISNULL([LineRate],0),ISNULL([LineAmount],0),ISNULL([LineGST],0) FROM @QuotationDetail

				COMMIT TRANSACTION
			END TRY

			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		END
	IF @OpMode = 2
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
										
					UPDATE QuotationJobSheet SET [CustomerID] = @CustomerID,[ItemID] = @ItemID,[ItemDescription] = @ItemDescription,[TaxID] = @TaxID
												,[GSTPercentage] = @GSTPercentage,[GSTWOPaper] = @GSTWOPaper
												,[CGSTAmount] = @CGSTAmount,[SGSTAmount] = @SGSTAmount,[IGSTAmount] = @IGSTAmount
												,[TaxableAmount] = @TaxableAmount,[Discount] = @Discount,[TotalBillingAmount] = @TotalBillingAmount
												,[AmountPayable] = @AmountPayable
												,[RoundOff] = @RoundOff
												,[OutstandingAmount] = @AmountPayable
												,[RecMode] = 'E'
												,LMB = @RequestBy
												,LMD = GETDATE()
					WHERE QuotationID = @QuotationID

					DELETE FROM QuotationJobSheetDetail WHERE QuotationID = @QuotationID

					INSERT INTO QuotationJobSheetDetail([QuotationID],[PrintComponentID],[NoOfSheet],[GSMID],[GSMValue]
													  ,[PaperSizeID],[PaperLength],[PaperWidth],[PaperTypeID],[PaperManufaturerID]
													  ,[NoOfSheetPrintDetail],[PaperSizeIDPrintDetail]
													  ,[Impression],[CuttingID],[NoofCuttingPices]
													  ,[FinishSize],[SideID],[ColourID],[PlateTypeID],[MachineID],[FabricationItemID],[FabricationItemSize]
													  ,[PackagingItemID],[LabourCharge],[TransportCharge],[TransportUnitID],[OperatorID],[Narration]
													  ,[LineQuantity],[LineRate],[LineAmount],[LineGST])

					SELECT @QuotationID,[PrintComponentID],[NoOfSheet],ISNULL([GSMID],0),ISNULL([GSMValue],0)
							,ISNULL([PaperSizeID],0),ISNULL([PaperLength],0),ISNULL([PaperWidth],0),ISNULL([PaperTypeID],0),ISNULL([PaperManufaturerID],0)
							,ISNULL([NoOfSheetPrintDetail],0),ISNULL([PaperSizeIDPrintDetail],0)
							,ISNULL([Impression],0),ISNULL([CuttingID],0),ISNULL([NoofCuttingPices],0)
							,[FinishSize],ISNULL([SideID],0),ISNULL([ColourID],0),[PlateTypeID],[MachineID],[FabricationItemID],[FabricationItemSize]
							,ISNULL([PackagingItemID],0),ISNULL([LabourCharge],0),ISNULL([TransportCharge],0),ISNULL([TransportUnitID],0),ISNULL([OperatorID],0),[Narration]
							,ISNULL([LineQuantity],0),ISNULL([LineRate],0),ISNULL([LineAmount],0),ISNULL([LineGST],0) FROM @QuotationDetail

				COMMIT TRANSACTION
			END TRY

			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		END
	IF @OpMode = 3
		BEGIN
			BEGIN TRY
				BEGIN TRANSACTION
					UPDATE QuotationJobSheet SET RecMode = 'D',DeleteBy = @RequestBy,DeletedDate = GETDATE() WHERE QuotationID = @QuotationID
				COMMIT TRANSACTION
			END TRY

			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		END
	IF @OpMode = 5 -- PRINT BILL
		BEGIN
					-- PAPER DETAIL
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CONVERT(VARCHAR,D.GSMValue) + ',' + CONVERT(VARCHAR,D.NoOfSheet) + ',' + CONVERT(VARCHAR,D.PaperLength) + 'x' + CONVERT(VARCHAR,D.PaperWidth) 
				+ CASE WHEN D.PaperTypeID > 0 THEN (SELECT ', ' + UPPER(T.PrintingParameterName) FROM PrintingParameter T WHERE PrintingParameterID = D.PaperTypeID) ELSE '' END
				+ CASE WHEN D.PaperManufaturerID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PaperManufaturerID) ELSE '' END
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.GSMValue > 0 OR D.NoOfSheet > 0 AND H.QuotationID = @QuotationID
		UNION -- PRINTING ITEM
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CONVERT(VARCHAR,CONVERT(INT,D.Impression))
				+ CASE WHEN D.PaperSizeIDPrintDetail > 0 THEN (SELECT ', ' + UPPER(p.PrintingParameterName) FROM PrintingParameter p WHERE PrintingParameterID = D.PaperSizeIDPrintDetail) ELSE '' END
				+ CASE WHEN D.CuttingID > 0 THEN (SELECT ', ' + UPPER(C.PrintingParameterName) FROM PrintingParameter C WHERE PrintingParameterID = D.CuttingID) ELSE '' END
				+ CASE WHEN D.ColourID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.ColourID) ELSE '' END
				+ CASE WHEN D.PlateTypeID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.PlateTypeID) ELSE '' END
				+ CASE WHEN D.MachineID > 0 THEN (SELECT ', ' + UPPER(M.PrintingParameterName) FROM PrintingParameter M WHERE PrintingParameterID = D.MachineID) ELSE '' END
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.CuttingID > 0 AND H.QuotationID = @QuotationID


		UNION -- FABRICATION ITEM
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CASE WHEN D.FabricationItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
				FROM PrintingParameter p WHERE PrintingParameterID = D.FabricationItemID) ELSE '' END
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.FabricationItemSize,''))
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
					+ ',' + CONVERT(VARCHAR,ISNULL(D.LineRate,0))
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.FabricationItemID > 0 AND H.QuotationID = @QuotationID

		UNION -- PACKING ITEM
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CASE WHEN D.PackagingItemID > 0 THEN (SELECT UPPER(p.PrintingParameterName) 
				FROM PrintingParameter p WHERE PrintingParameterID = D.PackagingItemID) ELSE '' END
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineQuantity,0))
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0))
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.PackagingItemID > 0 AND H.QuotationID = @QuotationID

		UNION -- LABOUR ITEM
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CONVERT(VARCHAR(50),ISNULL(D.LabourCharge,0))
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.TransportCharge,0))
					+ CASE WHEN D.TransportUnitID > 0 THEN (SELECT ',' + UPPER(p.PrintingParameterName) 
						FROM PrintingParameter p WHERE PrintingParameterID = D.TransportUnitID) ELSE '' END
					+ ',' + CONVERT(VARCHAR(50),ISNULL(D.LineRate,0))
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.LabourCharge > 0 AND H.QuotationID = @QuotationID

		UNION -- OPERATOR
		SELECT H.QuotationNO,H.QuotationDate,C.CustomerName,C.[Address],C.GSTNo,LOWER(C.Email) AS Email,C.Telephone
			,CASE WHEN H.ItemID > 0 THEN (SELECT I.PrintingParameterName FROM PrintingParameter I WHERE PrintingParameterID = H.ItemID) ELSE '' END AS Item
			,H.ItemDescription
			,CASE WHEN H.TaxID > 0 THEN (SELECT T.PrintingParameterName FROM PrintingParameter T WHERE PrintingParameterID = H.TaxID) ELSE '' END AS Tax
			,H.GSTPercentage,H.GSTWOPaper,H.TaxableAmount,H.TotalBillingAmount,H.Discount,H.RoundOff,H.AmountPayable
			,D.PrintComponentID
			,CASE WHEN D.OperatorID > 0 THEN (SELECT UPPER(p.PrintingParameterName) + ', ' + UPPER(D.Narration) + ', ' + CONVERT(VARCHAR(50),D.LineAmount)
				FROM PrintingParameter p WHERE PrintingParameterID = D.OperatorID) ELSE '' END
					AS LineDescription
					,D.LineAmount
						FROM QuotationJobSheet H 
							INNER JOIN QuotationJobSheetDetail D ON H.QuotationID = D.QuotationID
								LEFT JOIN Customer C ON H.CustomerID = C.CustomerID
									WHERE D.OperatorID > 0 AND H.QuotationID = @QuotationID
		END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_MANAGE_PRINT_RATE]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_MANAGE_PRINT_RATE]
	@PrintRateList PRINT_RATE READONLY
	,@CuttingSizeID INT = 161
	,@RequestBy INT = 0
	,@OpMode INT = 4
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			IF @OpMode = 4
				BEGIN
					IF EXISTS(SELECT * FROM PrintRateCalculation WHERE CuttingSizeID = @CuttingSizeID)
						BEGIN
							;WITH CTE_LIST(PlateID,Plate,ColorID,Color,PlateCost1K,ColorCost1K,PlateCost10K,ColorCost10K) AS
							(SELECT PR.PlateID,CASE WHEN PR.PlateID > 0 THEN (SELECT PrintingParameterName 
								FROM PrintingParameter WHERE PrintingParameterID = PR.PlateID) ELSE '' END AS Plate
								,PR.ColorID
								,CASE WHEN PR.ColorID > 0 THEN (SELECT UPPER(PrintingParameterName) 
									FROM PrintingParameter WHERE PrintingParameterID = PR.ColorID) ELSE '' END AS Color
								,PR.PlateCost1K,PR.ColorCost1K,PR.PlateCost10K,PR.ColorCost10K 
								FROM PrintRateCalculation PR WHERE CuttingSizeID = @CuttingSizeID) SELECT * FROM CTE_LIST P ORDER BY P.Plate,P.Color
						END
					ELSE
						BEGIN
							;WITH CTE_PLATE(PlateID,Plate) AS
								(SELECT p.PrintingParameterID,p.PrintingParameterName from PrintingParameter P
									WHERE P.PrintingParameterTypeID = 4 AND P.RecMode <> 'D') SELECT P.PlateID,P.Plate,C.PrintingParameterID As ColorID,UPPER(C.PrintingParameterName) As Color
										,0 AS PlateCost1K,0 As ColorCost1K,0 AS PlateCost10K,0 As ColorCost10K
											FROM CTE_PLATE P CROSS JOIN PrintingParameter C WHERE C.PrintingParameterTypeID = 7 AND C.RecMode <> 'D'
												ORDER BY P.Plate,C.PrintingParameterName
						END
				END
			ELSE IF @OpMode = 1
				BEGIN
					DELETE FROM PrintRateCalculation WHERE CuttingSizeID = @CuttingSizeID
					INSERT INTO PrintRateCalculation(CuttingSizeID,PlateID,ColorID,PlateCost1K,ColorCost1K,PlateCost10K,ColorCost10K,RecMode,UploadBy,UploadDate)
					SELECT @CuttingSizeID, PlateID, ColorID,PlateCost1K, ColorCost1K, PlateCost10K, ColorCost10K,'A',@RequestBy,GETDATE() FROM @PrintRateList
				END
			
		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[USP_PAYMENT]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_PAYMENT
CREATE PROCEDURE [dbo].[USP_PAYMENT]
	@PaymentID BIGINT = 0
	,@PaymentDate DATE
	,@CustomerID INT = 0
	,@PaymentMode INT = 0
	,@PaidAmount DECIMAL(18 ,2) = 0
	,@PaymentDetail VARCHAR(500) = ''
	,@RequestBy INT = 0
	,@FinYear VARCHAR(50) = ''
	,@OpMode INT = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @QuotationID BIGINT = 0,@OutstandingAmount DECIMAL(18 ,2) = 0,@RevertPayment DECIMAL(18, 2) = 0
		IF @OpMode = 1
			BEGIN
				DECLARE @PaymentNumber VARCHAR(50) = (SELECT 'P' + '/' + @FinYear + '/' + RIGHT('00000'+ CONVERT(NVARCHAR(10),ISNULL(COUNT(*),0) + 1),5) FROM Payment WHERE FinYear = @FinYear)
				INSERT INTO Payment([PaymentNumber],[PaymentDate],[CustomerID],[PaymentMode],[PaidAmount]
									,[PaymentDetail],[FinYear],[RecMode],[CreateBy],[CreateDate])
				VALUES(@PaymentNumber,@PaymentDate,@CustomerID,@PaymentMode,@PaidAmount,@PaymentDetail,@FinYear,'A',@RequestBy,GETDATE())

				SET @PaymentID = SCOPE_IDENTITY()

				DECLARE CUR_QUOTATION_PAYMENT CURSOR FOR (SELECT QuotationID,OutstandingAmount FROM QuotationJobSheet WHERE CustomerID = @CustomerID AND OutstandingAmount > 0 AND RecMode <> 'D')
					OPEN CUR_QUOTATION_PAYMENT
						FETCH NEXT FROM CUR_QUOTATION_PAYMENT INTO @QuotationID,@OutstandingAmount
							IF @@FETCH_STATUS <> 0 PRINT '<NONE>' WHILE @@FETCH_STATUS = 0
								BEGIN
									IF @PaidAmount > @OutstandingAmount
										BEGIN
											INSERT INTO PaymentDetail(PaymentID,QuotationID,Amount)
												VALUES(@PaymentID,@QuotationID,@OutstandingAmount)

											UPDATE QuotationJobSheet SET AmountPaid += @OutstandingAmount,OutstandingAmount -= @OutstandingAmount
												WHERE QuotationID = @QuotationID

											SET @PaidAmount -= @OutstandingAmount
										END
									ELSE
										BEGIN
											INSERT INTO PaymentDetail(PaymentID,QuotationID,Amount)
												VALUES(@PaymentID,@QuotationID,@PaidAmount)

											UPDATE QuotationJobSheet SET AmountPaid += @PaidAmount,OutstandingAmount -= @PaidAmount
												WHERE QuotationID = @QuotationID

											SET @PaidAmount = 0
											BREAK
										END

							FETCH NEXT FROM CUR_QUOTATION_PAYMENT INTO @QuotationID,@OutstandingAmount
						END
					CLOSE CUR_QUOTATION_PAYMENT
				DEALLOCATE CUR_QUOTATION_PAYMENT
			END
		ELSE IF @OpMode = 2
			BEGIN
				DECLARE CUR_REVERT_PAYMENT CURSOR FOR (SELECT  QuotationID,Amount FROM PaymentDetail WHERE PaymentID = @PaymentID)
					OPEN CUR_REVERT_PAYMENT
						FETCH NEXT FROM CUR_REVERT_PAYMENT INTO @QuotationID,@RevertPayment
							IF @@FETCH_STATUS <> 0 PRINT '<NONE>' WHILE @@FETCH_STATUS = 0
								BEGIN
									UPDATE QuotationJobSheet SET AmountPaid -= @RevertPayment,OutstandingAmount += @RevertPayment
										WHERE QuotationID = @QuotationID

									SET @QuotationID = 0
									SET @RevertPayment = 0	
							FETCH NEXT FROM CUR_REVERT_PAYMENT INTO @QuotationID,@RevertPayment
						END
					CLOSE CUR_REVERT_PAYMENT
				DEALLOCATE CUR_REVERT_PAYMENT
				
				UPDATE Payment SET [PaymentDate] = @PaymentDate,[PaymentMode] = @PaymentMode,[PaidAmount] = @PaidAmount
									,[PaymentDetail] = @PaymentDetail,[RecMode] = 'E',LMB = @RequestBy, LMD = GETDATE()
				WHERE PaymentID = @PaymentID
				
				DELETE FROM PaymentDetail WHERE PaymentID = @PaymentID

				DECLARE CUR_QUOTATION_PAYMENT CURSOR FOR (SELECT QuotationID,OutstandingAmount FROM QuotationJobSheet WHERE CustomerID = @CustomerID AND OutstandingAmount > 0 AND RecMode <> 'D')
					OPEN CUR_QUOTATION_PAYMENT
						FETCH NEXT FROM CUR_QUOTATION_PAYMENT INTO @QuotationID,@OutstandingAmount
							IF @@FETCH_STATUS <> 0 PRINT '<NONE>' WHILE @@FETCH_STATUS = 0
								BEGIN
									IF @PaidAmount > @OutstandingAmount
										BEGIN
											INSERT INTO PaymentDetail(PaymentID,QuotationID,Amount)
												VALUES(@PaymentID,@QuotationID,@OutstandingAmount)

											UPDATE QuotationJobSheet SET AmountPaid += @OutstandingAmount,OutstandingAmount -= @OutstandingAmount
												WHERE QuotationID = @QuotationID

											SET @PaidAmount -= @OutstandingAmount
											SET @OutstandingAmount = 0
											SET @QuotationID = 0
										END
									ELSE
										BEGIN
											INSERT INTO PaymentDetail(PaymentID,QuotationID,Amount)
												VALUES(@PaymentID,@QuotationID,@PaidAmount)

											UPDATE QuotationJobSheet SET AmountPaid += @PaidAmount,OutstandingAmount -= @PaidAmount
												WHERE QuotationID = @QuotationID

											SET @PaidAmount = 0
											BREAK
										END

							FETCH NEXT FROM CUR_QUOTATION_PAYMENT INTO @QuotationID,@OutstandingAmount
						END
					CLOSE CUR_QUOTATION_PAYMENT
				DEALLOCATE CUR_QUOTATION_PAYMENT
			END
		ELSE IF @OpMode = 3
			BEGIN
				DECLARE CUR_REVERT_PAYMENT CURSOR FOR (SELECT QuotationID,Amount FROM PaymentDetail WHERE PaymentID = @PaymentID)
					OPEN CUR_REVERT_PAYMENT
						FETCH NEXT FROM CUR_REVERT_PAYMENT INTO @QuotationID,@RevertPayment
							IF @@FETCH_STATUS <> 0 PRINT '<NONE>' WHILE @@FETCH_STATUS = 0
								BEGIN
									UPDATE QuotationJobSheet SET AmountPaid -= @RevertPayment,OutstandingAmount += @RevertPayment
										WHERE QuotationID = @QuotationID

									SET @QuotationID = 0
									SET @RevertPayment = 0	
							FETCH NEXT FROM CUR_REVERT_PAYMENT INTO @QuotationID,@RevertPayment
						END
					CLOSE CUR_REVERT_PAYMENT
				DEALLOCATE CUR_REVERT_PAYMENT
				
				UPDATE Payment SET [RecMode] = 'D',DeleteBy = @RequestBy, DeleteDate = GETDATE()
				WHERE PaymentID = @PaymentID
				
				DELETE FROM PaymentDetail WHERE PaymentID = @PaymentID
			END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SEARCH_PAYMENT_LIST]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec USP_SEARCH_PAYMENT_LIST '18-19','- Select -','','',''
CREATE PROCEDURE [dbo].[USP_SEARCH_PAYMENT_LIST]
(
@CurrentSession NVARCHAR(50),
@SearchColumn NVARCHAR(50),
@SearchText NVARCHAR(50),
@FromDate NVARCHAR(50),
@ToDate NVARCHAR(50)
)
AS
BEGIN
	if(@SearchColumn='- Select -')
	set @SearchColumn='P.PaymentNumber'
	else if(@SearchColumn='PAYMENT NO')
	set @SearchColumn='P.PaymentNumber'
	else if(@SearchColumn='DATE')
	set @SearchColumn='P.PaymentDate'
	else if(@SearchColumn='CUSTOMER')
	set @SearchColumn='C.CustomerName'

	SET NOCOUNT ON;
	IF @SearchColumn = '' BEGIN SET @SearchColumn = 'P.PaymentNumber' END;
		DECLARE @SQL_FORMATTED_QUERY NVARCHAR(MAX);
	
		SET @SQL_FORMATTED_QUERY = 'SELECT P.PaymentID,P.PaymentNumber,P.PaymentDate,C.CustomerName,P.PaidAmount
										,CASE WHEN P.PaymentMode > 0 THEN (SELECT PMode FROM PaymentMode WHERE PaymentModeID = P.PaymentMode) ELSE '''' END AS PMode,P.PaymentDetail 
											,P.PaymentDetail 
												FROM Payment P INNER JOIN Customer C ON P.CustomerID = C.CustomerID
													WHERE P.FinYear = ''' + CONVERT(VARCHAR(50),@CurrentSession) + ''' AND P.RecMode <> ''D'''
									
			IF @SearchColumn = 'P.PaymentDate'
					BEGIN
						SET @SQL_FORMATTED_QUERY = @SQL_FORMATTED_QUERY + ' AND CONVERT(DATE,'+ @SearchColumn +',103) BETWEEN CONVERT(DATE,''' + @FromDate + ''',103) AND CONVERT(DATE,''' + @ToDate + ''',103) ORDER BY P.PaymentNumber,P.PaymentDate DESC'
					END
				ELSE
					BEGIN							
						SET @SQL_FORMATTED_QUERY = @SQL_FORMATTED_QUERY + ' AND ' + @SearchColumn + ' LIKE ''%' + @SearchText + '%'''  + ' AND P.FinYear = ''' + CONVERT(VARCHAR(50),@CurrentSession) + ''' ORDER BY P.PaymentNumber,P.PaymentDate DESC'
					END
		EXEC(@SQL_FORMATTED_QUERY)
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SEARCH_QUOTATION_LIST]    Script Date: 7/27/2018 1:46:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec USP_SEARCH_QUOTATION_LIST '18-19','- Select -','','',''
CREATE PROCEDURE [dbo].[USP_SEARCH_QUOTATION_LIST]
(
@CurrentSession NVARCHAR(50),
@SearchColumn NVARCHAR(50),
@SearchText NVARCHAR(50),
@FromDate NVARCHAR(50),
@ToDate NVARCHAR(50)
)
AS
BEGIN
	if(@SearchColumn='- Select -')
	set @SearchColumn='Q.QuotationNO'
	else if(@SearchColumn='QUOTATION NO')
	set @SearchColumn='Q.QuotationNO'
	else if(@SearchColumn='DATE')
	set @SearchColumn='Q.QuotationDate'
	else if(@SearchColumn='CUSTOMER')
	set @SearchColumn='C.CustomerName'

	SET NOCOUNT ON;
	IF @SearchColumn = '' BEGIN SET @SearchColumn = 'Q.QuotationNO' END;
		DECLARE @SQL_FORMATTED_QUERY NVARCHAR(MAX);
	
		SET @SQL_FORMATTED_QUERY = 'SELECT Q.QuotationID,Q.QuotationNO,Q.QuotationDate
										,CASE WHEN ISNULL(Q.ItemID,0) > 0 THEN (SELECT TOP 1 PrintingParameterName FROM PrintingParameter WHERE PrintingParameterID = Q.ItemID AND PrintingParameterTypeID = 2003) ELSE '''' END AS Item
										,Q.ItemDescription
										,Q.TotalBillingAmount,Q.Discount,Q.TaxableAmount,Q.AmountPayable,Q.AmountPaid,Q.OutstandingAmount
											,C.CustomerName,C.GSTNo,C.Telephone 
												FROM QuotationJobSheet Q INNER JOIN Customer C ON Q.CustomerID = C.CustomerID
									WHERE Q.FinYear = ''' + CONVERT(VARCHAR(50),@CurrentSession) + ''' AND Q.RecMode <> ''D'''
									
			IF @SearchColumn = 'Q.QuotationDate'
					BEGIN
						SET @SQL_FORMATTED_QUERY = @SQL_FORMATTED_QUERY + ' AND CONVERT(DATE,'+ @SearchColumn +',103) BETWEEN CONVERT(DATE,''' + @FromDate + ''',103) AND CONVERT(DATE,''' + @ToDate + ''',103) ORDER BY Q.QuotationNO,Q.QuotationDate DESC'
					END
				ELSE
					BEGIN							
						SET @SQL_FORMATTED_QUERY = @SQL_FORMATTED_QUERY + ' AND ' + @SearchColumn + ' LIKE ''%' + @SearchText + '%'''  + ' AND Q.FinYear = ''' + CONVERT(VARCHAR(50),@CurrentSession) + ''' ORDER BY Q.QuotationNO,Q.QuotationDate DESC'
					END
			
		EXEC(@SQL_FORMATTED_QUERY)
	SET NOCOUNT OFF;
END
GO
