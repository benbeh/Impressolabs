/*Users*/
CREATE TRIGGER updateUserTrigger
BEFORE UPDATE ON aspnetusers
FOR EACH ROW
  SET NEW.LastUpdate=NOW();

/*Company*/
CREATE TRIGGER updateCompanyTrigger
BEFORE UPDATE ON companies
FOR EACH ROW
  SET NEW.LastUpdate=NOW();

/* 
що не додано до бд
1) AppUserJob (DateOfPost) default value NOW()
2) Company (LastUpdate) trigger and default value NOW()
*/