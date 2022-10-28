-- Start:: 1. Get the list of not discontinued products including category name ordered by product name.
SELECT 
	P.*,
	C.CategoryName
FROM 
	dbo.Products P
	INNER JOIN dbo.Categories C ON C.CategoryID = P.CategoryID
WHERE
	P.Discontinued = 0
ORDER BY P.ProductName;
-- End:: 1. Get the list of not discontinued products including category name ordered by product name.


-- Start:: 2. Get all Nancy Davolio’s customers.
SELECT DISTINCT
	C.*,
	E.FirstName,
	E.LastName
FROM 
	dbo.Orders O
	INNER JOIN dbo.Employees E ON E.EmployeeID = O.EmployeeID
	INNER JOIN dbo.Customers C ON C.CustomerID = O.CustomerID
WHERE
	E.FirstName = 'Nancy' 
	AND E.LastName = 'Davolio';

-- End:: 2. Get all Nancy Davolio’s customers.


-- Start:: 3. Get the total ordered amount (money) by year of the employee Steven Buchanan.
SELECT 
	CONCAT(MIN(E.FirstName), ' ', MIN(E.LastName)) AS EmployeeName,
	YEAR(O.OrderDate) AS [Year],	
	SUM(ISNULL(OD.UnitPrice, 0) * ISNULL(OD.Quantity, 0)) AS OrderAmount,
	ROUND(SUM((ISNULL(OD.UnitPrice, 0) * ISNULL(OD.Quantity, 0)) - (((ISNULL(OD.UnitPrice, 0) * ISNULL(OD.Quantity, 0)) * ISNULL(OD.Discount, 0)) / 100)), 2) AS OrderAmountAfterDiscont
FROM 
	dbo.[Order Details] OD
	INNER JOIN dbo.Orders O ON O.OrderID = OD.OrderID
	INNER JOIN dbo.Employees E ON E.EmployeeID = O.EmployeeID
WHERE
	E.FirstName = 'Steven' 
	AND E.LastName = 'Buchanan'
GROUP BY YEAR(O.OrderDate);
-- End:: 3. Get the total ordered amount (money) by year of the employee Steven Buchanan.


-- Start:: 4. Get the name of all employees that directly or indirectly report to Andrew Fuller.
;WITH CTEEmp
AS
(
	SELECT
		E.*
	FROM
		dbo.Employees E
		INNER JOIN dbo.Employees E1 ON E1.EmployeeID = E.ReportsTo
	WHERE
		E1.FirstName = 'Andrew'
		AND E1.LastName = 'Fuller'
	UNION ALL
	SELECT
		E.*
	FROM
		dbo.Employees E
		INNER JOIN CTEEmp E1 ON E1.EmployeeID = E.ReportsTo
)
SELECT * FROM CTEEmp;
-- End:: 4. Get the name of all employees that directly or indirectly report to Andrew Fuller.

