SELECT gr."Title", u."FullName", gr."GroupId"
FROM "UserToUserGroup"  uu
JOIN "UserGroups" gr ON uu."UserGroupId" = gr."Id"
JOIN "Users" u ON u."Id" = uu."UserId"
WHERE 1 =1
AND u."Id" IN (SELECT "RightUserId" FROM "Users"
			  JOIN "FriendsUserToUsers"
			  ON "Users"."Id" = "FriendsUserToUsers"."LeftUserId"
			  WHERE "Users"."UserId" = 263667943)
AND LOWER(gr."Title") like '%%' 
ORDER BY gr."GroupId";