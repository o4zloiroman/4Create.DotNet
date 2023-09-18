CREATE TABLE IF NOT EXISTS Employee 
(
    id UUID PRIMARY KEY,
    title TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS Company 
(
    id UUID PRIMARY KEY,
    name TEXT UNIQUE NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS EmployeeCompany 
(
    employee_id UUID REFERENCES Employee(id),
    company_id UUID REFERENCES Company(id),
    PRIMARY KEY (employee_id, company_id)
);

CREATE TABLE IF NOT EXISTS SystemLog 
(
    resource_type TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    event TEXT NOT NULL,
    changeset TEXT NOT NULL,
    comment TEXT
);

CREATE INDEX IF NOT EXISTS idx_employee_id ON EmployeeCompany (employee_id);
CREATE INDEX IF NOT EXISTS idx_company_id ON EmployeeCompany (company_id);
