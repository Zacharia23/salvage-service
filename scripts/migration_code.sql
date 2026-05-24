CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory"
(
    "MigrationId"
    character
    varying
(
    150
) NOT NULL,
    "ProductVersion" character varying
(
    32
) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY
(
    "MigrationId"
)
    );

START TRANSACTION;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetRoles"
(
    "Id"               text NOT NULL,
    "Name"             character varying(256),
    "NormalizedName"   character varying(256),
    "ConcurrencyStamp" text,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Categories"
(
    "Id"          uuid                     NOT NULL,
    "Name"        text                     NOT NULL,
    "Description" text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Companies"
(
    "Id"            uuid                     NOT NULL,
    "Number"        text                     NOT NULL,
    "Name"          text                     NOT NULL,
    "ContactPerson" text                     NOT NULL,
    "Phone"         text                     NOT NULL,
    "Email"         text                     NOT NULL,
    "Location"      text                     NOT NULL,
    "Status"        integer                  NOT NULL,
    "CompanyType"   integer                  NOT NULL,
    "LogoUrl"       text                     NOT NULL,
    "CreatedDate"   timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Companies" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Currencies"
(
    "Id"          uuid                     NOT NULL,
    "Name"        text                     NOT NULL,
    "Symbol"      text                     NOT NULL,
    "Code"        text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Currencies" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "IdentityTypes"
(
    "Id"          uuid                     NOT NULL,
    "Name"        text                     NOT NULL,
    "Description" text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_IdentityTypes" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Makes"
(
    "Id"          uuid                     NOT NULL,
    "Name"        text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Makes" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "MessageLogs"
(
    "Id"             uuid                     NOT NULL,
    "Phone"          character varying(100)   NOT NULL,
    "RequestId"      character varying(100)   NOT NULL,
    "DeliveryStatus" character varying(100)   NOT NULL,
    "Content"        character varying(500)   NOT NULL,
    "CreatedDate"    timestamp with time zone NOT NULL,
    CONSTRAINT "PK_MessageLogs" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Questions"
(
    "Id"          uuid                     NOT NULL,
    "Question"    text                     NOT NULL,
    "Answer"      text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "UpdatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Questions" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Receipts"
(
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_Receipts" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Regions"
(
    "Id"          uuid                     NOT NULL,
    "RegionIso"   text                     NOT NULL,
    "RegionName"  text                     NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Regions" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "SpareParts"
(
    "Id"          uuid                     NOT NULL,
    "PartNumber"  character varying(100)   NOT NULL,
    "Name"        character varying(100)   NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_SpareParts" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "SystemUsers"
(
    "SystemUserId" uuid                     NOT NULL,
    "Username"     text                     NOT NULL,
    "Number"       text                     NOT NULL,
    "Email"        text                     NOT NULL,
    "Phone"        text                     NOT NULL,
    "Address"      text                     NOT NULL,
    "Role"         text                     NOT NULL,
    "Status"       integer                  NOT NULL,
    "CreatedDate"  timestamp with time zone NOT NULL,
    CONSTRAINT "PK_SystemUsers" PRIMARY KEY ("SystemUserId")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Templates"
(
    "Id"          uuid                     NOT NULL,
    "Name"        character varying(100)   NOT NULL,
    "Channel"     integer                  NOT NULL,
    "Content"     character varying(1000)  NOT NULL,
    "Subject"     text                     NOT NULL,
    "IsActive"    boolean                  NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Templates" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "WorkingInfo"
(
    "Id"    uuid NOT NULL,
    "Title" text NOT NULL,
    CONSTRAINT "PK_WorkingInfo" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetRoleClaims"
(
    "Id"         integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId"     text NOT NULL,
    "ClaimType"  text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Models"
(
    "Id"          uuid                     NOT NULL,
    "Name"        character varying(200)   NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "MakeId"      uuid                     NOT NULL,
    CONSTRAINT "PK_Models" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Models_Makes_MakeId" FOREIGN KEY ("MakeId") REFERENCES "Makes" ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Customers"
(
    "Id"              uuid                     NOT NULL,
    "FirstName"       text                     NOT NULL,
    "LastName"        text                     NOT NULL,
    "Email"           text                     NOT NULL,
    "Phone"           text                     NOT NULL,
    "AccountType"     integer,
    "IdentityTypeId"  uuid,
    "CardNumber"      text                     NOT NULL,
    "Gender"          integer,
    "BirthDate"       timestamp with time zone,
    "AcceptedTerms"   boolean                  NOT NULL,
    "AccountVerified" boolean                  NOT NULL,
    "RegionId"        uuid,
    "TaxNumber"       text                     NOT NULL,
    "VNumber"         text                     NOT NULL,
    "CreatedDate"     timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Customers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Customers_IdentityTypes_IdentityTypeId" FOREIGN KEY ("IdentityTypeId") REFERENCES "IdentityTypes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Customers_Regions_RegionId" FOREIGN KEY ("RegionId") REFERENCES "Regions" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetUsers"
(
    "Id"                   text    NOT NULL,
    "SystemUserId"         uuid    NOT NULL,
    "Domain"               text,
    "UserName"             character varying(256),
    "NormalizedUserName"   character varying(256),
    "Email"                character varying(256),
    "NormalizedEmail"      character varying(256),
    "EmailConfirmed"       boolean NOT NULL,
    "PasswordHash"         text,
    "SecurityStamp"        text,
    "ConcurrencyStamp"     text,
    "PhoneNumber"          text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled"     boolean NOT NULL,
    "LockoutEnd"           timestamp with time zone,
    "LockoutEnabled"       boolean NOT NULL,
    "AccessFailedCount"    integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUsers_SystemUsers_SystemUserId" FOREIGN KEY ("SystemUserId") REFERENCES "SystemUsers" ("SystemUserId") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Sections"
(
    "Id"            uuid NOT NULL,
    "Content"       text NOT NULL,
    "WorkingInfoId" uuid,
    CONSTRAINT "PK_Sections" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Sections_WorkingInfo_WorkingInfoId" FOREIGN KEY ("WorkingInfoId") REFERENCES "WorkingInfo" ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Vehicles"
(
    "Id"                 uuid                     NOT NULL,
    "CompanyId"          uuid                     NOT NULL,
    "Reference"          text                     NOT NULL,
    "RegistrationNumber" text                     NOT NULL,
    "Title"              text                     NOT NULL,
    "Description"        text                     NOT NULL,
    "Reserved"           boolean                  NOT NULL,
    "MakeId"             uuid                     NOT NULL,
    "ModelId"            uuid                     NOT NULL,
    "Year"               text,
    "Mileage"            text,
    "Engine"             text,
    "TitleStatus"        text,
    "RegionId"           uuid,
    "BodyStyle"          integer                  NOT NULL,
    "Drive"              integer                  NOT NULL,
    "Transmission"       integer                  NOT NULL,
    "ExteriorColor"      text,
    "InteriorColor"      text,
    "Highlights"         text,
    "Issues"             text,
    "LastService"        timestamp with time zone,
    "SellerNotes"        text,
    "CreatedDate"        timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Vehicles_Companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Vehicles_Makes_MakeId" FOREIGN KEY ("MakeId") REFERENCES "Makes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Vehicles_Models_ModelId" FOREIGN KEY ("ModelId") REFERENCES "Models" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Vehicles_Regions_RegionId" FOREIGN KEY ("RegionId") REFERENCES "Regions" ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "ActivityLogs"
(
    "Id"         uuid                     NOT NULL,
    "CustomerId" uuid                     NOT NULL,
    "EventType"  integer                  NOT NULL,
    "Content"    text                     NOT NULL,
    "Agent"      text                     NOT NULL,
    "IpAddress"  text                     NOT NULL,
    "Timestamp"  timestamp with time zone NOT NULL,
    CONSTRAINT "PK_ActivityLogs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ActivityLogs_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Notifications"
(
    "Id"         uuid                     NOT NULL,
    "Title"      text                     NOT NULL,
    "Message"    text                     NOT NULL,
    "Type"       integer                  NOT NULL,
    "Target"     integer                  NOT NULL,
    "CustomerId" uuid,
    "Created"    timestamp with time zone NOT NULL,
    "Scheduled"  timestamp with time zone,
    "IsSent"     boolean                  NOT NULL,
    "Delivered"  timestamp with time zone,
    CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Notifications_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetUserClaims"
(
    "Id"         integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId"     text NOT NULL,
    "ClaimType"  text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetUserLogins"
(
    "LoginProvider"       text NOT NULL,
    "ProviderKey"         text NOT NULL,
    "ProviderDisplayName" text,
    "UserId"              text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetUserRoles"
(
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "AspNetUserTokens"
(
    "UserId"        text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name"          text NOT NULL,
    "Value"         text,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Offers"
(
    "Id"              uuid                     NOT NULL,
    "OfferNature"     integer                  NOT NULL,
    "EntityType"      integer                  NOT NULL,
    "ReferenceNumber" character varying(100)   NOT NULL,
    "VehicleId"       uuid,
    "SparePartId"     uuid,
    "IncrementPrice"  double precision         NOT NULL,
    "ReservePrice"    double precision         NOT NULL,
    "Views"           integer                  NOT NULL,
    "StartDate"       timestamp with time zone NOT NULL,
    "EndDate"         timestamp with time zone NOT NULL,
    "Extended"        boolean                  NOT NULL,
    "CreatedDate"     timestamp with time zone NOT NULL,
    "Status"          integer                  NOT NULL,
    CONSTRAINT "PK_Offers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Offers_SpareParts_SparePartId" FOREIGN KEY ("SparePartId") REFERENCES "SpareParts" ("Id"),
    CONSTRAINT "FK_Offers_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES "Vehicles" ("Id")
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "VehicleImages"
(
    "Id"        uuid NOT NULL,
    "ImageUrl"  text NOT NULL,
    "VehicleId" uuid NOT NULL,
    CONSTRAINT "PK_VehicleImages" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_VehicleImages_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES "Vehicles" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "CustomerNotifications"
(
    "Id"             uuid                     NOT NULL,
    "CustomerId"     uuid                     NOT NULL,
    "NotificationId" uuid                     NOT NULL,
    "IsRead"         boolean                  NOT NULL,
    "ReadTime"       timestamp with time zone,
    "ReceivedTime"   timestamp with time zone NOT NULL,
    CONSTRAINT "PK_CustomerNotifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CustomerNotifications_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CustomerNotifications_Notifications_NotificationId" FOREIGN KEY ("NotificationId") REFERENCES "Notifications" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Bids"
(
    "Id"              uuid                     NOT NULL,
    "OfferId"         uuid                     NOT NULL,
    "BidReference"    character varying(100)   NOT NULL,
    "CustomerId"      uuid                     NOT NULL,
    "PreviousAmount"  numeric(18, 2)           NOT NULL,
    "SubmittedAmount" numeric(18, 2)           NOT NULL,
    "Awarded"         boolean                  NOT NULL,
    "CreatedDate"     timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Bids" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Bids_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Bids_Offers_OfferId" FOREIGN KEY ("OfferId") REFERENCES "Offers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Subscriptions"
(
    "Id"          uuid                     NOT NULL,
    "CustomerId"  uuid                     NOT NULL,
    "OfferId"     uuid                     NOT NULL,
    "IsActive"    boolean                  NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Subscriptions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Subscriptions_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Subscriptions_Offers_OfferId" FOREIGN KEY ("OfferId") REFERENCES "Offers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE TABLE "Invoices"
(
    "Id"          uuid                     NOT NULL,
    "Reference"   character varying(100)   NOT NULL,
    "CustomerId"  uuid                     NOT NULL,
    "BidId"       uuid                     NOT NULL,
    "Amount"      numeric                  NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Invoices" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Invoices_Bids_BidId" FOREIGN KEY ("BidId") REFERENCES "Bids" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Invoices_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
    VALUES ('71f40cfb-69f8-4fd5-8a1c-4c1437a2f1c2', NULL, 'Customer', 'CUSTOMER');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('7221d870-628f-4cce-b06e-000d95ca5e31', NULL, 'Manager', 'MANAGER');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('93b32d27-e30d-4e91-a170-b256c1654c6f', NULL, 'Developer', 'DEVELOPER');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('d5de92f7-0aab-45b2-a94d-9c49c76b9c92', NULL, 'Administrator', 'ADMINISTRATOR');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "Categories" ("Id", "CreatedDate", "Description", "Name")
    VALUES ('0198eec5-eb20-790e-85d3-3cf7618158b2', TIMESTAMPTZ '2025-08-28T03:43:24.960115Z', 'Normal Vehicle', 'Normal');
INSERT INTO "Categories" ("Id", "CreatedDate", "Description", "Name")
VALUES ('0198eec5-eb20-7a0f-a49a-c480d3d33601', TIMESTAMPTZ '2025-08-28T03:43:24.960073Z', 'Damaged Vehicle',
        'Damaged');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "Currencies" ("Id", "Code", "CreatedDate", "Name", "Symbol")
    VALUES ('0198eec5-eb20-75e3-b5fd-7d031b16c94c', 'USD', TIMESTAMPTZ '2025-08-28T03:43:24.960342Z', 'US Dollar', '$');
INSERT INTO "Currencies" ("Id", "Code", "CreatedDate", "Name", "Symbol")
VALUES ('0198eec5-eb20-762f-a5b5-5774989f990f', 'TZS', TIMESTAMPTZ '2025-08-28T03:43:24.960328Z', 'Tanzanian Shillings',
        '/=');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "IdentityTypes" ("Id", "CreatedDate", "Description", "Name")
    VALUES ('0198eec5-eb1f-74a5-8a1d-9574994bfd79', TIMESTAMPTZ '2025-08-28T03:43:24.959436Z', 'National Identification Number', 'NIN');
INSERT INTO "IdentityTypes" ("Id", "CreatedDate", "Description", "Name")
VALUES ('0198eec5-eb1f-7850-b76e-ea643388c4f5', TIMESTAMPTZ '2025-08-28T03:43:24.959468Z', 'Drivers Licence',
        'Drivers Licence');
INSERT INTO "IdentityTypes" ("Id", "CreatedDate", "Description", "Name")
VALUES ('0198eec5-eb1f-7c5e-9798-90aa34d0fde6', TIMESTAMPTZ '2025-08-28T03:43:24.959468Z', 'Passport Number',
        'Passport Number');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
    VALUES ('0198eec5-eb20-710b-a5d2-ea210859a95b', TIMESTAMPTZ '2025-08-28T03:43:24.960245Z', 'RV21', 'Ruvuma');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-72ce-82ac-1ba5072e66c4', TIMESTAMPTZ '2025-08-28T03:43:24.960239Z', 'MBY14', 'Mbeya');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-72d5-b868-2d3b94a93b4c', TIMESTAMPTZ '2025-08-28T03:43:24.960242Z', 'PN09', 'Pemba North');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7383-8965-da5311f32fd3', TIMESTAMPTZ '2025-08-28T03:43:24.960237Z', 'MYR26', 'Manyara');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-73b9-af76-674bd11a0b49', TIMESTAMPTZ '2025-08-28T03:43:24.960241Z', 'NJ29', 'Njombe');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7426-9aaa-be5de5810567', TIMESTAMPTZ '2025-08-28T03:43:24.960243Z', 'PWN19', 'Pwani');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-743d-947d-3e1df6195929', TIMESTAMPTZ '2025-08-28T03:43:24.960191Z', 'DOM03', 'Dodoma');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-745b-97d0-bbebc11b2500', TIMESTAMPTZ '2025-08-28T03:43:24.960244Z', 'RK20', 'Rukwa');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-74a3-9d48-19cd508fba3a', TIMESTAMPTZ '2025-08-28T03:43:24.960195Z', 'KLM09', 'Kilimanjaro');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7559-8ed5-ae84ba52486c', TIMESTAMPTZ '2025-08-28T03:43:24.960152Z', 'ARS01', 'Arusha');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-75ba-8039-23bd11971dc4', TIMESTAMPTZ '2025-08-28T03:43:24.960246Z', 'SMY30', 'Simiyu');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-75e6-a2cc-efa478b5244e', TIMESTAMPTZ '2025-08-28T03:43:24.960243Z', 'PS10', 'Pemba South');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7620-bff7-825d5b649487', TIMESTAMPTZ '2025-08-28T03:43:24.960247Z', 'TBR24', 'Tabora');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-76b9-9f0f-da83d8b855d2', TIMESTAMPTZ '2025-08-28T03:43:24.960249Z', 'ZSC11',
        'Zanzibar South and Central');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7732-b8c3-25f19edc112d', TIMESTAMPTZ '2025-08-28T03:43:24.960238Z', 'MAR13', 'Mara');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-777a-bfd9-5ea17d36a4c0', TIMESTAMPTZ '2025-08-28T03:43:24.960192Z', 'IRA27', 'Iringa');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7863-8ed2-7efe4f67c19a', TIMESTAMPTZ '2025-08-28T03:43:24.960245Z', 'SHY22', 'Shinyanga');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-78e4-b296-e873b05fbbf9', TIMESTAMPTZ '2025-08-28T03:43:24.960194Z', 'KTV28', 'Katavi');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7974-a719-8634147d6b5f', TIMESTAMPTZ '2025-08-28T03:43:24.960239Z', 'MOR16', 'Morogoro');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-79b5-a170-bd74e246592b', TIMESTAMPTZ '2025-08-28T03:43:24.960192Z', 'GTA27', 'Geita');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-79bb-b57f-92527edf05dc', TIMESTAMPTZ '2025-08-28T03:43:24.960248Z', 'TNG25', 'Tanga');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-79c7-abbd-9bec7b4d98ef', TIMESTAMPTZ '2025-08-28T03:43:24.96024Z', 'MTR17', 'Mtwara');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7a57-928c-562756edc3e5', TIMESTAMPTZ '2025-08-28T03:43:24.960247Z', 'SNG23', 'Singida');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7b03-815b-2aa98cbb43d0', TIMESTAMPTZ '2025-08-28T03:43:24.96025Z', 'ZW15', 'Zanzibar West');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7b61-b664-1ab9a633a880', TIMESTAMPTZ '2025-08-28T03:43:24.960194Z', 'KGM08', 'Kigoma');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7ba3-9ae9-28e7fefee693', TIMESTAMPTZ '2025-08-28T03:43:24.960193Z', 'KGR27', 'Kagera');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7c06-a2a6-f55d7b496ebc', TIMESTAMPTZ '2025-08-28T03:43:24.960249Z', 'ZN07', 'Zanzibar North');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7c29-a720-5657b4ffaf9d', TIMESTAMPTZ '2025-08-28T03:43:24.960196Z', 'LND12', 'Lindi');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7e4c-bffb-cc24fc0af410', TIMESTAMPTZ '2025-08-28T03:43:24.960241Z', 'MZA18', 'Mwanza');
INSERT INTO "Regions" ("Id", "CreatedDate", "RegionIso", "RegionName")
VALUES ('0198eec5-eb20-7eab-9773-3b9316d56043', TIMESTAMPTZ '2025-08-28T03:43:24.96019Z', 'DSM02', 'Dar es Salaam');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_ActivityLogs_CustomerId" ON "ActivityLogs" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_AspNetUsers_SystemUserId" ON "AspNetUsers" ("SystemUserId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Bids_CustomerId" ON "Bids" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Bids_OfferId" ON "Bids" ("OfferId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_CustomerNotifications_CustomerId" ON "CustomerNotifications" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_CustomerNotifications_NotificationId" ON "CustomerNotifications" ("NotificationId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Customers_IdentityTypeId" ON "Customers" ("IdentityTypeId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Customers_RegionId" ON "Customers" ("RegionId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Invoices_BidId" ON "Invoices" ("BidId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Invoices_CustomerId" ON "Invoices" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Models_MakeId" ON "Models" ("MakeId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Notifications_CustomerId" ON "Notifications" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Offers_SparePartId" ON "Offers" ("SparePartId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE UNIQUE INDEX "IX_Offers_VehicleId" ON "Offers" ("VehicleId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Sections_WorkingInfoId" ON "Sections" ("WorkingInfoId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Subscriptions_CustomerId" ON "Subscriptions" ("CustomerId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Subscriptions_OfferId" ON "Subscriptions" ("OfferId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_VehicleImages_VehicleId" ON "VehicleImages" ("VehicleId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Vehicles_CompanyId" ON "Vehicles" ("CompanyId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Vehicles_MakeId" ON "Vehicles" ("MakeId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Vehicles_ModelId" ON "Vehicles" ("ModelId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
CREATE INDEX "IX_Vehicles_RegionId" ON "Vehicles" ("RegionId");
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828034325_InitialMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250828034325_InitialMigration', '9.0.9');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "AspNetRoles"
WHERE "Id" = '71f40cfb-69f8-4fd5-8a1c-4c1437a2f1c2';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "AspNetRoles"
WHERE "Id" = '7221d870-628f-4cce-b06e-000d95ca5e31';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "AspNetRoles"
WHERE "Id" = '93b32d27-e30d-4e91-a170-b256c1654c6f';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "AspNetRoles"
WHERE "Id" = 'd5de92f7-0aab-45b2-a94d-9c49c76b9c92';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Categories"
WHERE "Id" = '0198eec5-eb20-790e-85d3-3cf7618158b2';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Categories"
WHERE "Id" = '0198eec5-eb20-7a0f-a49a-c480d3d33601';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Currencies"
WHERE "Id" = '0198eec5-eb20-75e3-b5fd-7d031b16c94c';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Currencies"
WHERE "Id" = '0198eec5-eb20-762f-a5b5-5774989f990f';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "IdentityTypes"
WHERE "Id" = '0198eec5-eb1f-74a5-8a1d-9574994bfd79';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "IdentityTypes"
WHERE "Id" = '0198eec5-eb1f-7850-b76e-ea643388c4f5';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "IdentityTypes"
WHERE "Id" = '0198eec5-eb1f-7c5e-9798-90aa34d0fde6';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-710b-a5d2-ea210859a95b';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-72ce-82ac-1ba5072e66c4';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-72d5-b868-2d3b94a93b4c';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7383-8965-da5311f32fd3';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-73b9-af76-674bd11a0b49';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7426-9aaa-be5de5810567';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-743d-947d-3e1df6195929';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-745b-97d0-bbebc11b2500';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-74a3-9d48-19cd508fba3a';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7559-8ed5-ae84ba52486c';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-75ba-8039-23bd11971dc4';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-75e6-a2cc-efa478b5244e';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7620-bff7-825d5b649487';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-76b9-9f0f-da83d8b855d2';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7732-b8c3-25f19edc112d';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-777a-bfd9-5ea17d36a4c0';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7863-8ed2-7efe4f67c19a';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-78e4-b296-e873b05fbbf9';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7974-a719-8634147d6b5f';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-79b5-a170-bd74e246592b';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-79bb-b57f-92527edf05dc';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-79c7-abbd-9bec7b4d98ef';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7a57-928c-562756edc3e5';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7b03-815b-2aa98cbb43d0';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7b61-b664-1ab9a633a880';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7ba3-9ae9-28e7fefee693';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7c06-a2a6-f55d7b496ebc';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7c29-a720-5657b4ffaf9d';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7e4c-bffb-cc24fc0af410';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
DELETE
FROM "Regions"
WHERE "Id" = '0198eec5-eb20-7eab-9773-3b9316d56043';
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123002140_InitialMigrationII') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123002140_InitialMigrationII', '9.0.9');
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123084617_CodeMigration') THEN
ALTER TABLE "AspNetUsers"
    ADD "AccountVerified" boolean NOT NULL DEFAULT FALSE;
END IF;
END $EF$;

DO
$EF$
BEGIN
    IF
NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251123084617_CodeMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251123084617_CodeMigration', '9.0.9');
END IF;
END $EF$;
COMMIT;

