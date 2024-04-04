START TRANSACTION;

INSERT INTO Users (Id, AccessFlags, UserName, NormalizedUserName)
VALUES (UUID(), 1, 'rooter', 'root');

INSERT INTO Tests (Id, CreatorId, TestName)
VALUES (UUID(),
        (SELECT Users.Id FROM Users WHERE Users.UserName = 'rooter' LIMIT 1),
        'simple name for Testname');

INSERT INTO ThemeTasks (Id, Theme)
VALUES (UUID(), 'тестовые задание');

INSERT INTO Tasks (Id, Question, InteractionType, User, TestId)
VALUES (UUID(), 'Some test question', 1,
        (SELECT Users.Id FROM Users WHERE Users.UserName = 'rooter' LIMIT 1),
        (SELECT Tests.Id FROM Tests WHERE Tests.TestName = 'simple name for Testname' LIMIT 1));

COMMIT;