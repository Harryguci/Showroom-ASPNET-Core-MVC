USE SHOWROOMAUTO;

Go

UPDATE Product_Images
SET Url_image = '/images/uploaded/toyota-camry.jpg'
WHERE id = 1;

UPDATE Product_Images
SET Url_image = '/images/uploaded/honda-civic.jpg'
WHERE id = 2;

UPDATE Product_Images
SET Url_image = '/images/uploaded/ford-mustang.jpg'
WHERE id = 3;

UPDATE Product_Images
SET Url_image = '/images/uploaded/chevrolet-silverado.jpg'
WHERE id = 4;

UPDATE Product_Images
SET Url_image = '/images/uploaded/audi-q5.jpg'
WHERE id = 5;

UPDATE Product_Images
SET Url_image = '/images/uploaded/mercedes-e200.jpg'
WHERE id = 6;

UPDATE Product_Images
SET Url_image = '/images/uploaded/bmw-x5.jpg'
WHERE id = 7;

UPDATE Product_Images
SET Url_image = '/images/uploaded/tesla-model3.jpg'
WHERE id = 8;

UPDATE Product_Images
SET Url_image = '/images/uploaded/nissan.jpg'
WHERE id = 9;

UPDATE Product_Images
SET Url_image = '/images/uploaded/kia-optima.jpg'
WHERE id = 10;

SET IDENTITY_INSERT Product_Images ON 


INSERT INTO PRODUCT_IMAGES ("id", "serial", "url_image") 
values (11, 'P011', '/images/uploaded/gzu6lb8m.png'),
(12, 'P012', '/images/uploaded/mercedes-s450.jpg'),
(13, 'P012', '/images/uploaded/mercedes-s450-2.jpg'),
(14, 'P012', '/images/uploaded/mercedes-s450-3.jpg')

GO

SELECT * FROM Product_Images RIGHT JOIN Products ON Product_Images.Serial = Products.Serial