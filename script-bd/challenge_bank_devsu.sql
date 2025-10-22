
-- ============================================================
-- Devsu Challenge - PostgreSQL schema (tbl_* standard) + seed
-- EF Core-aligned + created_at/updated_at on client & account
-- ============================================================

DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'challenge_bank_devsu') THEN
    PERFORM dblink_exec('dbname=' || current_database(), ''); -- no-op para asegurar extensión dblink si la usas
    EXECUTE 'CREATE DATABASE challenge_bank_devsu';
  END IF;
END$$;


CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Idempotent drops (FK order)
DROP TABLE IF EXISTS tbl_move CASCADE;
DROP TABLE IF EXISTS tbl_account CASCADE;
DROP TABLE IF EXISTS tbl_log CASCADE;
DROP TABLE IF EXISTS tbl_client CASCADE;

-- ============================================================
-- Helper: trigger to auto-update 'updated_at'
-- ============================================================
CREATE OR REPLACE FUNCTION set_updated_at()
RETURNS trigger AS $$
BEGIN
  NEW.updated_at := now();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- =====================
-- tbl_client
-- =====================
CREATE TABLE tbl_client (
    client_id              uuid           NOT NULL DEFAULT gen_random_uuid(),
    full_name              varchar(120)   NOT NULL,
    gender                 varchar(2)     NOT NULL,
    age                    int            NOT NULL,
    identification_number  varchar(20)    NOT NULL,
    address                text           NOT NULL,
    phone_number           varchar(15)    NOT NULL,
    password               varchar(20)    NOT NULL,
    active                 boolean        NOT NULL DEFAULT true,
    created_at             timestamptz    NOT NULL DEFAULT now(),
    updated_at             timestamptz    NOT NULL DEFAULT now(),
    CONSTRAINT tbl_client_pkey PRIMARY KEY (client_id)
);
CREATE UNIQUE INDEX ux_tbl_client_identification_number ON tbl_client(identification_number);
CREATE INDEX ix_tbl_client_active ON tbl_client(active);
CREATE INDEX ix_tbl_client_full_name ON tbl_client(full_name);

-- Trigger to maintain updated_at
CREATE TRIGGER trg_tbl_client_updated_at
BEFORE UPDATE ON tbl_client
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

-- =====================
-- tbl_account
-- =====================
CREATE TABLE tbl_account (
    account_id      uuid           NOT NULL DEFAULT gen_random_uuid(),
    account_number  text           NOT NULL,
    account_type    int            NOT NULL,     -- 1=Ahorros, 2=Corriente
    initial_balance numeric(18,2)  NOT NULL DEFAULT 0,
    active          boolean        NOT NULL DEFAULT true,
    client_ref_id   uuid           NOT NULL,
    created_at      timestamptz    NOT NULL DEFAULT now(),
    updated_at      timestamptz    NOT NULL DEFAULT now(),
    CONSTRAINT tbl_account_pkey PRIMARY KEY (account_id),
    CONSTRAINT fk_tbl_account_client FOREIGN KEY (client_ref_id)
        REFERENCES tbl_client(client_id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ux_tbl_account_account_number ON tbl_account(account_number);
CREATE INDEX ix_tbl_account_client ON tbl_account(client_ref_id);
CREATE INDEX ix_tbl_account_active ON tbl_account(active);

-- Trigger to maintain updated_at
CREATE TRIGGER trg_tbl_account_updated_at
BEFORE UPDATE ON tbl_account
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

-- =====================
-- tbl_move
-- =====================
CREATE TABLE tbl_move (
    move_id          uuid           NOT NULL DEFAULT gen_random_uuid(),
    transaction_date timestamptz    NOT NULL DEFAULT now(),
    move_type        int            NOT NULL,    -- 1=Crédito, 2=Débito
    amount           numeric(18,2)  NOT NULL,
    balance  		 numeric(18,2)  NOT NULL,
    account_ref_id   uuid           NOT NULL,
    success          boolean        NOT NULL,
    CONSTRAINT tbl_move_pkey PRIMARY KEY (move_id),
    CONSTRAINT fk_tbl_move_account FOREIGN KEY (account_ref_id)
        REFERENCES tbl_account(account_id) ON DELETE CASCADE
);
CREATE INDEX ix_tbl_move_account_date ON tbl_move(account_ref_id, transaction_date);
CREATE INDEX ix_tbl_move_type ON tbl_move(move_type);

-- =====================
-- tbl_log
-- =====================
CREATE TABLE tbl_log (
    log_id      bigserial    NOT NULL,
    resource_id uuid         NULL,
    message     text         NOT NULL,
    created_at        timestamptz  NULL DEFAULT now(),
    CONSTRAINT tbl_log_pkey PRIMARY KEY (log_id)
);
CREATE INDEX ix_tbl_log_resource ON tbl_log(resource_id);
CREATE INDEX ix_tbl_log_date ON tbl_log(date);

