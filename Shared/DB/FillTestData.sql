START TRANSACTION;

INSERT INTO Users (Id, AccessFlags, UserName, NormalizedUserName, PasswordHash)
VALUES (UUID(), 1, 'root', 'rooter', 'ABEAgd8WihbmgXMY9Xp+ZMPg4Xt1YK66UFSw2e4ObQKlX3VfnM//72gWUA6Uq6f1LA==');
COMMIT;

INSERT INTO Tests (Id, CreatorId, TestName, CreationTime)
VALUES (UUID(),
        (SELECT Users.Id FROM Users WHERE Users.UserName = 'rooter' LIMIT 1),
        'simple name for Testname',
        current_date());
COMMIT;

INSERT INTO ThemeTasks (Id, Theme)
VALUES (UUID(), 'тестовые задание');
COMMIT;

INSERT INTO Tasks (Id, Question, InteractionType, User, TestId)
VALUES (UUID(), 'Some test question', 1,
        (SELECT Users.Id FROM Users WHERE Users.UserName = 'rooter' LIMIT 1),
        (SELECT Tests.Id FROM Tests WHERE Tests.TestName = 'simple name for Testname' LIMIT 1));
COMMIT;