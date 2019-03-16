ALTER TABLE QuotationJobSheet ADD Status INT NULL
ALTER TABLE PAYMENT ADD JobSheetType INT NULL

USE [TechPrint]
GO
/****** Object:  StoredProcedure [dbo].[USP_PAYMENT]    Script Date: 17-03-2019 00:18:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_PAYMENT
ALTER PROCEDURE [dbo].[USP_PAYMENT]
	@PaymentID BIGINT = 0
	,@PaymentDate DATE
	,@CustomerID INT = 0
	,@PaymentMode INT = 0
	,@PaidAmount DECIMAL(18 ,2) = 0
	,@PaymentDetail VARCHAR(500) = ''
	,@RequestBy INT = 0
	,@FinYear VARCHAR(50) = ''
	,@OpMode INT = 1
	,@JobSheetType INT = 0
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
									,[PaymentDetail],[FinYear],[RecMode],[CreateBy],[CreateDate], [JobSheetType])
				VALUES(@PaymentNumber,@PaymentDate,@CustomerID,@PaymentMode,@PaidAmount,@PaymentDetail,@FinYear,'A',@RequestBy,GETDATE(), @JobSheetType)

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




