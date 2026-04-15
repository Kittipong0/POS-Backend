-- ============================================================
-- POS Database Full Reseed Script (v2 - MenuItemIngredients)
-- ============================================================

-- 1. Clear all data (respect FK order)
DELETE FROM MenuItemIngredients;
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM Inventories;
DELETE FROM MenuItems;
DELETE FROM InventoryCategories;
DELETE FROM MenuCategories;
DELETE FROM Tables;

-- Reset identity seeds
DBCC CHECKIDENT ('MenuItemIngredients', RESEED, 0);
DBCC CHECKIDENT ('OrderItems', RESEED, 0);
DBCC CHECKIDENT ('Orders', RESEED, 0);
DBCC CHECKIDENT ('Inventories', RESEED, 0);
DBCC CHECKIDENT ('MenuItems', RESEED, 0);
DBCC CHECKIDENT ('InventoryCategories', RESEED, 0);
DBCC CHECKIDENT ('MenuCategories', RESEED, 0);
DBCC CHECKIDENT ('Tables', RESEED, 0);

-- ============================================================
-- 2. MenuCategories (4 categories)
-- ============================================================
SET IDENTITY_INSERT MenuCategories ON;
INSERT INTO MenuCategories (Id, Name) VALUES
(1, N'อาหารจานหลัก'),
(2, N'เครื่องดื่ม'),
(3, N'ของหวาน'),
(4, N'เมนูพิเศษ');
SET IDENTITY_INSERT MenuCategories OFF;

-- ============================================================
-- 3. InventoryCategories (6 categories)
-- ============================================================
SET IDENTITY_INSERT InventoryCategories ON;
INSERT INTO InventoryCategories (Id, Name) VALUES
(1, N'เนื้อสัตว์'),
(2, N'ผัก/ผลไม้'),
(3, N'เครื่องปรุง'),
(4, N'แป้ง/ข้าว'),
(5, N'เครื่องดื่ม'),
(6, N'อื่นๆ');
SET IDENTITY_INSERT InventoryCategories OFF;

-- ============================================================
-- 4. MenuItems (25 items across 4 categories)
-- ============================================================
SET IDENTITY_INSERT MenuItems ON;

-- Category 1: อาหารจานหลัก (8 items)
INSERT INTO MenuItems (Id, Name, NameEn, Description, Price, CategoryId, IsAvailable, IsRecommended, Cost, SalesCount, Image) VALUES
(1,  N'ข้าวผัดกระเพราหมูสับ', N'Basil Pork Fried Rice',       N'ข้าวผัดกระเพราหมูสับไข่ดาว',                   65.00,  1, 1, 1, 25.00,  120, NULL),
(2,  N'ผัดไทยกุ้งสด',         N'Pad Thai Shrimp',              N'ผัดไทยเส้นจันท์กุ้งสดตัวใหญ่',                  89.00,  1, 1, 1, 35.00,  95,  NULL),
(3,  N'ข้าวมันไก่',           N'Hainanese Chicken Rice',       N'ข้าวมันไก่สูตรต้นตำรับ น้ำจิ้มรสเด็ด',          60.00,  1, 1, 0, 22.00,  80,  NULL),
(4,  N'ต้มยำกุ้งน้ำข้น',     N'Tom Yum Kung',                 N'ต้มยำกุ้งน้ำข้นรสเข้มข้น',                      120.00, 1, 1, 1, 50.00,  110, NULL),
(5,  N'แกงเขียวหวานไก่',     N'Green Curry Chicken',          N'แกงเขียวหวานไก่พร้อมข้าวสวย',                    75.00,  1, 1, 0, 30.00,  65,  NULL),
(6,  N'ข้าวคลุกกะปิ',         N'Shrimp Paste Fried Rice',      N'ข้าวคลุกกะปิกุ้งหวาน หมูหวาน',                  70.00,  1, 1, 0, 28.00,  45,  NULL),
(7,  N'สปาเก็ตตี้ผัดขี้เมา', N'Drunken Spaghetti',            N'สปาเก็ตตี้ผัดขี้เมาทะเล',                        85.00,  1, 1, 0, 32.00,  55,  NULL),
(8,  N'ข้าวหมูแดง',           N'Red BBQ Pork Rice',            N'ข้าวหมูแดงหมูกรอบ น้ำราดสูตรเด็ด',              65.00,  1, 1, 0, 25.00,  70,  NULL);

-- Category 2: เครื่องดื่ม (8 items)
INSERT INTO MenuItems (Id, Name, NameEn, Description, Price, CategoryId, IsAvailable, IsRecommended, Cost, SalesCount, Image) VALUES
(9,  N'ชาไทยเย็น',           N'Thai Iced Tea',                N'ชาไทยเย็นใส่นมข้นหวาน',                          35.00, 2, 1, 1,  8.00,  200, NULL),
(10, N'กาแฟเย็น',             N'Iced Coffee',                  N'กาแฟโบราณเย็นสูตรเข้มข้น',                        40.00, 2, 1, 1, 10.00,  180, NULL),
(11, N'น้ำมะนาวโซดา',         N'Lemon Soda',                   N'น้ำมะนาวโซดาสดชื่น',                              35.00, 2, 1, 0,  7.00,  90,  NULL),
(12, N'น้ำส้มคั้นสด',         N'Fresh Orange Juice',           N'น้ำส้มคั้นสด 100%',                               45.00, 2, 1, 0, 15.00,  60,  NULL),
(13, N'สมูทตี้มะม่วง',       N'Mango Smoothie',               N'สมูทตี้มะม่วงน้ำดอกไม้ปั่น',                      55.00, 2, 1, 1, 18.00,  75,  NULL),
(14, N'ชาเขียวมัทฉะ',         N'Matcha Green Tea',             N'ชาเขียวมัทฉะลาเต้เย็น',                            50.00, 2, 1, 0, 15.00,  55,  NULL),
(15, N'โกโก้เย็น',             N'Iced Cocoa',                   N'โกโก้เย็นเข้มข้น',                                  45.00, 2, 1, 0, 12.00,  40,  NULL),
(16, N'น้ำเปล่า',             N'Water',                        N'น้ำดื่มสะอาดขวดใหญ่',                              15.00, 2, 1, 0,  3.00,  300, NULL);

-- Category 3: ของหวาน (5 items)
INSERT INTO MenuItems (Id, Name, NameEn, Description, Price, CategoryId, IsAvailable, IsRecommended, Cost, SalesCount, Image) VALUES
(17, N'ข้าวเหนียวมะม่วง',     N'Mango Sticky Rice',            N'ข้าวเหนียวมูลมะม่วงน้ำดอกไม้',                    69.00, 3, 1, 1, 25.00, 85, NULL),
(18, N'ไอศกรีมกะทิ',           N'Coconut Ice Cream',            N'ไอศกรีมกะทิโรยถั่วลิสง',                            39.00, 3, 1, 0, 10.00, 50, NULL),
(19, N'บัวลอยไข่หวาน',         N'Bua Loy',                      N'บัวลอยไข่หวานน้ำกะทิอุ่นๆ',                          45.00, 3, 1, 0, 12.00, 35, NULL),
(20, N'เครปเค้กชาไทย',         N'Thai Tea Crepe Cake',          N'เครปเค้กชาไทยเนื้อนุ่ม',                              79.00, 3, 1, 1, 30.00, 60, NULL),
(21, N'โทสต์เนยน้ำผึ้ง',     N'Honey Butter Toast',           N'โทสต์กรอบราดน้ำผึ้งเนย',                              49.00, 3, 1, 0, 15.00, 40, NULL);

-- Category 4: เมนูพิเศษ (4 items)
INSERT INTO MenuItems (Id, Name, NameEn, Description, Price, CategoryId, IsAvailable, IsRecommended, Cost, SalesCount, Image) VALUES
(22, N'สเต็กเนื้อวากิว',     N'Wagyu Steak',                  N'สเต็กเนื้อวากิว A5 ย่างถ่าน',                      450.00, 4, 1, 1, 200.00, 30, NULL),
(23, N'ล็อบสเตอร์ย่างเนย',  N'Grilled Lobster',              N'ล็อบสเตอร์ย่างเนยกระเทียม',                        390.00, 4, 1, 1, 180.00, 25, NULL),
(24, N'พิซซ่าทรัฟเฟิล',     N'Truffle Pizza',                N'พิซซ่าครีมทรัฟเฟิลเห็ดรวม',                        280.00, 4, 1, 0, 100.00, 20, NULL),
(25, N'เซ็ตซาชิมิรวม',       N'Sashimi Set',                  N'เซ็ตซาชิมิรวมปลาสดนำเข้า',                          350.00, 4, 1, 1, 150.00, 35, NULL);

SET IDENTITY_INSERT MenuItems OFF;

-- ============================================================
-- 5. Inventories (standalone raw materials – NO MenuItemId)
-- ============================================================
SET IDENTITY_INSERT Inventories ON;

-- เนื้อสัตว์ (CategoryId = 1)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(1,  N'หมูสับ',       1, 5000,  1000, N'กรัม', 0.12, N'CP Foods',         '2026-02-23'),
(2,  N'เนื้อไก่',     1, 4000,  800,  N'กรัม', 0.10, N'CP Foods',         '2026-02-23'),
(3,  N'กุ้งสด',       1, 3000,  500,  N'กรัม', 0.35, N'ตลาดทะเลไทย',    '2026-02-23'),
(4,  N'ปลาหมึก',     1, 2000,  400,  N'กรัม', 0.25, N'ตลาดทะเลไทย',    '2026-02-23'),
(5,  N'ไข่ไก่',       1, 200,   50,   N'ฟอง', 4.00, N'CP Foods',         '2026-02-23'),
(6,  N'หมูแดง',       1, 3000,  500,  N'กรัม', 0.18, N'ร้านหมูแดงเจ๊กี',  '2026-02-23'),
(7,  N'หมูกรอบ',     1, 2000,  400,  N'กรัม', 0.20, N'ร้านหมูแดงเจ๊กี',  '2026-02-23'),
(8,  N'เนื้อวากิว A5', 1, 5000, 1000, N'กรัม', 3.50, N'นำเข้าญี่ปุ่น',     '2026-02-23'),
(9,  N'ล็อบสเตอร์สด', 1, 3000, 500,  N'กรัม', 2.50, N'ตลาดทะเลไทย',    '2026-02-23'),
(10, N'แซลมอนสด',   1, 3000,  300,  N'กรัม', 1.20, N'นำเข้าญี่ปุ่น',     '2026-02-23'),
(11, N'ทูน่าสด',     1, 2000,  200,  N'กรัม', 1.50, N'นำเข้าญี่ปุ่น',     '2026-02-23'),
(12, N'ปลาฮามาจิ',   1, 1500,  200,  N'กรัม', 1.80, N'นำเข้าญี่ปุ่น',     '2026-02-23');

-- ผัก/ผลไม้ (CategoryId = 2)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(13, N'ใบกะเพรา',     2, 2000,  500,  N'กรัม', 0.05, N'สวนผักบ้านนา',    '2026-02-23'),
(14, N'พริกขี้หนู',   2, 1000,  200,  N'กรัม', 0.08, N'สวนผักบ้านนา',    '2026-02-23'),
(15, N'กระเทียม',     2, 1500,  300,  N'กรัม', 0.06, N'สวนผักบ้านนา',    '2026-02-23'),
(16, N'หอมแดง',       2, 1000,  200,  N'กรัม', 0.07, N'สวนผักบ้านนา',    '2026-02-23'),
(17, N'มะนาว',       2, 300,   50,   N'ลูก', 3.00, N'สวนผักบ้านนา',    '2026-02-23'),
(18, N'ข่า',           2, 800,   200,  N'กรัม', 0.04, N'สวนผักบ้านนา',    '2026-02-23'),
(19, N'ตะไคร้',       2, 800,   200,  N'กรัม', 0.03, N'สวนผักบ้านนา',    '2026-02-23'),
(20, N'ใบมะกรูด',     2, 500,   100,  N'กรัม', 0.05, N'สวนผักบ้านนา',    '2026-02-23'),
(21, N'ผักบุ้ง',       2, 3000,  500,  N'กรัม', 0.03, N'สวนผักบ้านนา',    '2026-02-23'),
(22, N'มะเขือเทศ',   2, 1500,  300,  N'กรัม', 0.04, N'สวนผักบ้านนา',    '2026-02-23'),
(23, N'เห็ดฟาง',     2, 2000,  300,  N'กรัม', 0.08, N'สวนผักบ้านนา',    '2026-02-23'),
(24, N'มะม่วงน้ำดอกไม้', 2, 1500, 300, N'กรัม', 0.10, N'สวนผักบ้านนา', '2026-02-23'),
(25, N'ส้มสด',       2, 2000,  300,  N'กรัม', 0.06, N'สวนผักบ้านนา',    '2026-02-23');

-- เครื่องปรุง (CategoryId = 3)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(26, N'น้ำมันพืช',       3, 10000, 2000, N'มล.',  0.04, N'Makro',           '2026-02-23'),
(27, N'น้ำปลา',           3, 5000,  1000, N'มล.',  0.03, N'Makro',           '2026-02-23'),
(28, N'ซอสหอยนางรม',     3, 3000,  500,  N'มล.',  0.05, N'Makro',           '2026-02-23'),
(29, N'น้ำตาลปี๊บ',       3, 2000,  500,  N'กรัม', 0.04, N'Makro',           '2026-02-23'),
(30, N'พริกแกง',           3, 1500,  300,  N'กรัม', 0.10, N'แม่ประนอม',      '2026-02-23'),
(31, N'กะทิ',               3, 5000,  1000, N'มล.',  0.06, N'Makro',           '2026-02-23'),
(32, N'น้ำพริกเผา',       3, 1000,  200,  N'กรัม', 0.08, N'แม่ประนอม',      '2026-02-23'),
(33, N'กะปิ',               3, 1000,  200,  N'กรัม', 0.10, N'Makro',           '2026-02-23'),
(34, N'ซอสพริก',           3, 2000,  400,  N'มล.',  0.04, N'Makro',           '2026-02-23'),
(35, N'เกลือทะเล',         3, 3000,  500,  N'กรัม', 0.02, N'Makro',           '2026-02-23'),
(36, N'พริกไทย',           3, 1000,  200,  N'กรัม', 0.15, N'Makro',           '2026-02-23'),
(37, N'น้ำมันงา',           3, 1000,  200,  N'มล.',  0.12, N'Makro',           '2026-02-23'),
(38, N'น้ำผึ้ง',           3, 1000,  200,  N'มล.',  0.20, N'Makro',           '2026-02-23'),
(39, N'เนยสด',             3, 2000,  400,  N'กรัม', 0.15, N'Makro',           '2026-02-23'),
(40, N'น้ำมันทรัฟเฟิล',   3, 500,   100,  N'มล.',  2.00, N'นำเข้าอิตาลี',  '2026-02-23');

-- แป้ง/ข้าว (CategoryId = 4)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(41, N'ข้าวสวย',           4, 10000, 2000, N'กรัม', 0.02, N'ข้าวมาบุญครอง',  '2026-02-23'),
(42, N'เส้นผัดไทย',       4, 3000,  500,  N'กรัม', 0.04, N'Makro',           '2026-02-23'),
(43, N'เส้นสปาเก็ตตี้',   4, 3000,  500,  N'กรัม', 0.06, N'Makro',           '2026-02-23'),
(44, N'ข้าวเหนียว',       4, 5000,  1000, N'กรัม', 0.03, N'ข้าวมาบุญครอง',  '2026-02-23'),
(45, N'แป้งพิซซ่า',       4, 2000,  400,  N'กรัม', 0.10, N'Makro',           '2026-02-23'),
(46, N'ขนมปังโทสต์',     4, 1500,  300,  N'แผ่น', 2.00, N'เอสแอนด์พี',    '2026-02-23'),
(47, N'แป้งเครป',         4, 2000,  400,  N'กรัม', 0.08, N'Makro',           '2026-02-23'),
(48, N'แป้งบัวลอย',       4, 2000,  400,  N'กรัม', 0.05, N'Makro',           '2026-02-23');

-- เครื่องดื่ม (CategoryId = 5)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(49, N'ชาไทย (ผง)',       5, 2000,  500,  N'กรัม', 0.15, N'ชาตรามือ',       '2026-02-23'),
(50, N'นมข้นหวาน',         5, 3000,  500,  N'มล.',  0.05, N'Makro',           '2026-02-23'),
(51, N'นมสด',               5, 5000,  1000, N'มล.',  0.04, N'Makro',           '2026-02-23'),
(52, N'เมล็ดกาแฟคั่ว',   5, 2000,  400,  N'กรัม', 0.30, N'Makro',           '2026-02-23'),
(53, N'น้ำแข็ง',           5, 20000, 5000, N'กรัม', 0.01, N'โรงน้ำแข็ง',    '2026-02-23'),
(54, N'โซดา',               5, 3000,  500,  N'มล.',  0.03, N'Makro',           '2026-02-23'),
(55, N'ผงมัทฉะ',           5, 500,   100,  N'กรัม', 0.50, N'นำเข้าญี่ปุ่น',  '2026-02-23'),
(56, N'ผงโกโก้',           5, 800,   200,  N'กรัม', 0.25, N'Makro',           '2026-02-23');

-- อื่นๆ (CategoryId = 6)
INSERT INTO Inventories (Id, Name, CategoryId, Quantity, MinimumQuantity, Unit, CostPerUnit, Supplier, LastUpdated) VALUES
(57, N'ถั่วลิสง',         6, 2000,  500,  N'กรัม', 0.08, N'Makro',           '2026-02-23'),
(58, N'เต้าหู้',           6, 1500,  300,  N'กรัม', 0.06, N'Makro',           '2026-02-23'),
(59, N'กุ้งแห้ง',         6, 1000,  200,  N'กรัม', 0.30, N'Makro',           '2026-02-23'),
(60, N'ซีฟู้ดรวม',       6, 3000,  500,  N'กรัม', 0.40, N'ตลาดทะเลไทย',    '2026-02-23'),
(61, N'ชีสมอสซาเรลล่า', 6, 2000,  400,  N'กรัม', 0.30, N'Makro',           '2026-02-23'),
(62, N'น้ำดื่มขวด',       6, 500,   100,  N'ขวด', 5.00, N'สิงห์',           '2026-02-23'),
(63, N'ไอศกรีมกะทิ',     6, 2000,  400,  N'กรัม', 0.15, N'สวนทิพย์',       '2026-02-23'),
(64, N'ครีมชาไทย',         6, 2000,  400,  N'มล.',  0.10, N'ชาตรามือ',       '2026-02-23');

SET IDENTITY_INSERT Inventories OFF;

-- ============================================================
-- 6. MenuItemIngredients (many-to-many join: MenuItem ↔ Inventory)
-- ============================================================
SET IDENTITY_INSERT MenuItemIngredients ON;

-- 1. ข้าวผัดกระเพราหมูสับ (MenuItem 1)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(1,  1, 1,  150, N'กรัม'),   -- หมูสับ
(2,  1, 13, 30,  N'กรัม'),   -- ใบกะเพรา
(3,  1, 14, 10,  N'กรัม'),   -- พริกขี้หนู
(4,  1, 15, 15,  N'กรัม'),   -- กระเทียม
(5,  1, 5,  1,   N'ฟอง'),    -- ไข่ไก่
(6,  1, 41, 200, N'กรัม'),   -- ข้าวสวย
(7,  1, 26, 20,  N'มล.'),    -- น้ำมันพืช
(8,  1, 27, 15,  N'มล.'),    -- น้ำปลา
(9,  1, 28, 10,  N'มล.');    -- ซอสหอยนางรม

-- 2. ผัดไทยกุ้งสด (MenuItem 2)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(10, 2, 42, 200, N'กรัม'),   -- เส้นผัดไทย
(11, 2, 3,  100, N'กรัม'),   -- กุ้งสด
(12, 2, 5,  1,   N'ฟอง'),    -- ไข่ไก่
(13, 2, 58, 50,  N'กรัม'),   -- เต้าหู้
(14, 2, 57, 20,  N'กรัม'),   -- ถั่วลิสง
(15, 2, 27, 15,  N'มล.'),    -- น้ำปลา
(16, 2, 29, 15,  N'กรัม');   -- น้ำตาลปี๊บ

-- 3. ข้าวมันไก่ (MenuItem 3)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(17, 3, 2,  150, N'กรัม'),   -- เนื้อไก่
(18, 3, 41, 250, N'กรัม'),   -- ข้าวสวย
(19, 3, 15, 10,  N'กรัม'),   -- กระเทียม
(20, 3, 37, 5,   N'มล.');    -- น้ำมันงา

-- 4. ต้มยำกุ้งน้ำข้น (MenuItem 4)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(21, 4, 3,  200, N'กรัม'),   -- กุ้งสด
(22, 4, 18, 30,  N'กรัม'),   -- ข่า
(23, 4, 19, 20,  N'กรัม'),   -- ตะไคร้
(24, 4, 20, 5,   N'กรัม'),   -- ใบมะกรูด
(25, 4, 17, 2,   N'ลูก'),    -- มะนาว
(26, 4, 27, 20,  N'มล.'),    -- น้ำปลา
(27, 4, 22, 50,  N'กรัม'),   -- มะเขือเทศ
(28, 4, 31, 100, N'มล.'),    -- กะทิ
(29, 4, 32, 15,  N'กรัม');   -- น้ำพริกเผา

-- 5. แกงเขียวหวานไก่ (MenuItem 5)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(30, 5, 2,  150, N'กรัม'),   -- เนื้อไก่
(31, 5, 30, 30,  N'กรัม'),   -- พริกแกง
(32, 5, 31, 200, N'มล.'),    -- กะทิ
(33, 5, 27, 15,  N'มล.'),    -- น้ำปลา
(34, 5, 41, 200, N'กรัม');   -- ข้าวสวย

-- 6. ข้าวคลุกกะปิ (MenuItem 6)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(35, 6, 41, 200, N'กรัม'),   -- ข้าวสวย
(36, 6, 33, 20,  N'กรัม'),   -- กะปิ
(37, 6, 59, 30,  N'กรัม'),   -- กุ้งแห้ง
(38, 6, 5,  1,   N'ฟอง'),    -- ไข่ไก่
(39, 6, 26, 20,  N'มล.');    -- น้ำมันพืช

-- 7. สปาเก็ตตี้ผัดขี้เมา (MenuItem 7)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(40, 7, 43, 200, N'กรัม'),   -- เส้นสปาเก็ตตี้
(41, 7, 60, 150, N'กรัม'),   -- ซีฟู้ดรวม
(42, 7, 14, 10,  N'กรัม'),   -- พริกขี้หนู
(43, 7, 15, 15,  N'กรัม'),   -- กระเทียม
(44, 7, 13, 20,  N'กรัม'),   -- ใบกะเพรา
(45, 7, 26, 30,  N'มล.');    -- น้ำมันพืช

-- 8. ข้าวหมูแดง (MenuItem 8)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(46, 8, 6,  100, N'กรัม'),   -- หมูแดง
(47, 8, 7,  80,  N'กรัม'),   -- หมูกรอบ
(48, 8, 41, 250, N'กรัม'),   -- ข้าวสวย
(49, 8, 34, 20,  N'มล.');    -- ซอสพริก

-- 9. ชาไทยเย็น (MenuItem 9)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(50, 9, 49, 15,  N'กรัม'),   -- ชาไทย (ผง)
(51, 9, 50, 30,  N'มล.'),    -- นมข้นหวาน
(52, 9, 53, 200, N'กรัม');   -- น้ำแข็ง

-- 10. กาแฟเย็น (MenuItem 10)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(53, 10, 52, 15, N'กรัม'),   -- เมล็ดกาแฟคั่ว
(54, 10, 51, 50, N'มล.'),    -- นมสด
(55, 10, 53, 200, N'กรัม');  -- น้ำแข็ง

-- 11. น้ำมะนาวโซดา (MenuItem 11)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(56, 11, 17, 2,  N'ลูก'),    -- มะนาว
(57, 11, 54, 200, N'มล.'),   -- โซดา
(58, 11, 29, 20, N'กรัม'),   -- น้ำตาลปี๊บ
(59, 11, 53, 150, N'กรัม');  -- น้ำแข็ง

-- 12. น้ำส้มคั้นสด (MenuItem 12)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(60, 12, 25, 200, N'กรัม'),  -- ส้มสด
(61, 12, 53, 150, N'กรัม');  -- น้ำแข็ง

-- 13. สมูทตี้มะม่วง (MenuItem 13)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(62, 13, 24, 150, N'กรัม'),  -- มะม่วงน้ำดอกไม้
(63, 13, 51, 100, N'มล.'),   -- นมสด
(64, 13, 53, 200, N'กรัม');  -- น้ำแข็ง

-- 14. ชาเขียวมัทฉะ (MenuItem 14)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(65, 14, 55, 5,   N'กรัม'),  -- ผงมัทฉะ
(66, 14, 51, 100, N'มล.'),   -- นมสด
(67, 14, 53, 200, N'กรัม');  -- น้ำแข็ง

-- 15. โกโก้เย็น (MenuItem 15)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(68, 15, 56, 20,  N'กรัม'),  -- ผงโกโก้
(69, 15, 51, 100, N'มล.'),   -- นมสด
(70, 15, 53, 200, N'กรัม');  -- น้ำแข็ง

-- 16. น้ำเปล่า (MenuItem 16)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(71, 16, 62, 1,   N'ขวด');   -- น้ำดื่มขวด

-- 17. ข้าวเหนียวมะม่วง (MenuItem 17)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(72, 17, 44, 150, N'กรัม'),  -- ข้าวเหนียว
(73, 17, 24, 200, N'กรัม'),  -- มะม่วงน้ำดอกไม้
(74, 17, 31, 50,  N'มล.');   -- กะทิ

-- 18. ไอศกรีมกะทิ (MenuItem 18)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(75, 18, 63, 100, N'กรัม'),  -- ไอศกรีมกะทิ
(76, 18, 57, 20,  N'กรัม');  -- ถั่วลิสง

-- 19. บัวลอยไข่หวาน (MenuItem 19)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(77, 19, 48, 100, N'กรัม'),  -- แป้งบัวลอย
(78, 19, 5,  1,   N'ฟอง'),   -- ไข่ไก่
(79, 19, 31, 100, N'มล.');   -- กะทิ

-- 20. เครปเค้กชาไทย (MenuItem 20)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(80, 20, 47, 100, N'กรัม'),  -- แป้งเครป
(81, 20, 64, 50,  N'มล.'),   -- ครีมชาไทย
(82, 20, 5,  2,   N'ฟอง');   -- ไข่ไก่

-- 21. โทสต์เนยน้ำผึ้ง (MenuItem 21)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(83, 21, 46, 3,   N'แผ่น'),  -- ขนมปังโทสต์
(84, 21, 39, 30,  N'กรัม'),  -- เนยสด
(85, 21, 38, 20,  N'มล.');   -- น้ำผึ้ง

-- 22. สเต็กเนื้อวากิว (MenuItem 22)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(86, 22, 8,  250, N'กรัม'),  -- เนื้อวากิว A5
(87, 22, 35, 5,   N'กรัม'),  -- เกลือทะเล
(88, 22, 36, 3,   N'กรัม'),  -- พริกไทย
(89, 22, 39, 20,  N'กรัม');  -- เนยสด

-- 23. ล็อบสเตอร์ย่างเนย (MenuItem 23)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(90, 23, 9,  300, N'กรัม'),  -- ล็อบสเตอร์สด
(91, 23, 39, 30,  N'กรัม'),  -- เนยสด
(92, 23, 15, 15,  N'กรัม');  -- กระเทียม

-- 24. พิซซ่าทรัฟเฟิล (MenuItem 24)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(93, 24, 45, 200, N'กรัม'),  -- แป้งพิซซ่า
(94, 24, 40, 5,   N'มล.'),   -- น้ำมันทรัฟเฟิล
(95, 24, 61, 100, N'กรัม'),  -- ชีสมอสซาเรลล่า
(96, 24, 23, 50,  N'กรัม');  -- เห็ดฟาง

-- 25. เซ็ตซาชิมิรวม (MenuItem 25)
INSERT INTO MenuItemIngredients (Id, MenuItemId, InventoryId, QuantityUsed, Unit) VALUES
(97, 25, 10, 100, N'กรัม'),  -- แซลมอนสด
(98, 25, 11, 80,  N'กรัม'),  -- ทูน่าสด
(99, 25, 12, 70,  N'กรัม');  -- ปลาฮามาจิ

SET IDENTITY_INSERT MenuItemIngredients OFF;

-- ============================================================
-- 7. Tables (12 tables with varying capacity)
-- ============================================================
SET IDENTITY_INSERT Tables ON;
INSERT INTO Tables (Id, TableNumber, Capacity, IsOccupied, LastOrderTime, QrToken) VALUES
(1,  'T-01',   2,  0, NULL, NULL),
(2,  'T-02',   2,  0, NULL, NULL),
(3,  'T-03',   4,  0, NULL, NULL),
(4,  'T-04',   4,  0, NULL, NULL),
(5,  'T-05',   4,  0, NULL, NULL),
(6,  'T-06',   6,  0, NULL, NULL),
(7,  'T-07',   6,  0, NULL, NULL),
(8,  'T-08',   8,  0, NULL, NULL),
(9,  'T-09',   8,  0, NULL, NULL),
(10, 'T-10',   10, 0, NULL, NULL),
(11, 'VIP-01', 6,  0, NULL, NULL),
(12, 'VIP-02', 10, 0, NULL, NULL);
SET IDENTITY_INSERT Tables OFF;

PRINT N'Reseed completed successfully!';
PRINT N'MenuCategories:      4 rows';
PRINT N'InventoryCategories: 6 rows';
PRINT N'MenuItems:           25 rows';
PRINT N'Inventories:         64 raw materials';
PRINT N'MenuItemIngredients: 99 links';
PRINT N'Tables:              12 rows';
