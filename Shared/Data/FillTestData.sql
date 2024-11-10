START TRANSACTION;

INSERT INTO Users (Id, AccessFlags, UserName, NormalizedUserName, PasswordHash)
VALUES (UUID(), 1, 'root', 'rooter', '4813494d137e1631bba301d5acab6e7bb7aa74ce1185d456565ef51d737677b2');

INSERT INTO Users (Id, AccessFlags, UserName, NormalizedUserName, PasswordHash)
VALUES (UUID(), 0, 'student', 'student', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8');
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

INSERT INTO Tasks (Id, Question, InteractionType, TestId)
VALUES (UUID(), 'Some test question', 1,
        (SELECT Tests.Id FROM Tests WHERE Tests.TestName = 'simple name for Testname' LIMIT 1));
COMMIT;