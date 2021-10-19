SELECT Orders.Order_Id,Orders.Cust_Id,Orders.Date,Customers.Name,Order_Details.Quantity,Order_Details.Amount
FROM 
((Orders INNER JOIN Customers ON Orders.Cust_Id = Customers.Cust_Id)INNER JOIN Order_Details ON Orders.Order_Id = Order_Details.Order_Id)