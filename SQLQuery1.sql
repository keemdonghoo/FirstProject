SELECT @@SERVERNAME
        
CREATE TABLE Bookmark
(
  u_id         varchar(20) NOT NULL,
  m_uid        int         NOT NULL,
  b_uid        int         NOT NULL,
  b_isbookmark bit        ,
  CONSTRAINT PK_Bookmark PRIMARY KEY (u_id, m_uid)
)


CREATE TABLE Movie
(
  m_uid      int         NOT NULL,
  m_title    Varchar(20) NOT NULL,
  m_director varchar     NOT NULL,
  m_type     varchar     NOT NULL,
  m_date     Datetime    NOT NULL,
  m_rating   varchar     NOT NULL,
  m_runtime  int         NOT NULL,
  CONSTRAINT PK_Movie PRIMARY KEY (m_uid)
)


CREATE TABLE Review
(
  m_uid     int     NOT NULL,
  u_uid     int     NOT NULL,
  r_uid     int     NOT NULL IDENTITY(1,1),
  r_content varchar,
  r_date    DATE    NOT NULL,
  r_rate    int     NOT NULL,
  CONSTRAINT PK_Review PRIMARY KEY (m_uid, u_uid)
)



CREATE TABLE project_user
(
  u_uid      int         NOT NULL IDENTITY(1,1),
  u_id       varchar(20) NOT NULL,
  u_password varchar(20) NOT NULL,
  u_name     varchar(20) NOT NULL,
  u_phonenum varchar(20)     NOT NULL,
  u_level    int         NOT NULL DEFAULT 1,
  u_nickname varchar(20)     NOT NULL,
  u_isadmin  bit         NOT NULL DEFAULT 0,
  CONSTRAINT PK_User PRIMARY KEY (u_uid)
)
drop table Project_User

 INSERT INTO project_user (u_id, u_password, u_name, u_phonenum, u_nickname, u_isadmin)
VALUES
('admin', 'admin1234', '������', '01099999999', '������',3),
('kdh', 'kdh1234', '�赿��', '01091946962', '����', 0),

('kkk', 'kdh12345', '�赿��', '01091946962', '����', 0),
('hhh', 'kdh12344', '������', '01091946962', '����', 0),
('bbb', 'kdh12343', '������', '01091946962', '����', 0),
('zzz', 'kdh12342', '�����', '01091946962', '����', 0),
('nnn', 'kdh12341', '�����', '01091946962', '����', 0);

        
      select*from Project_User
	  select*from Review
	  select*from bookmark
	  select*from MovieList