--
-- PostgreSQL database dump
--

-- Dumped from database version 11.16
-- Dumped by pg_dump version 11.16

-- Started on 2022-05-30 14:53:37

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 2838 (class 0 OID 0)
-- Dependencies: 3
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 197 (class 1259 OID 16402)
-- Name: DescFile; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DescFile" (
    "Id" integer NOT NULL,
    "FileId" integer NOT NULL
);


ALTER TABLE public."DescFile" OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 16424)
-- Name: DescFile_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."DescFile" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."DescFile_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 196 (class 1259 OID 16394)
-- Name: File; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."File" (
    "Id" integer NOT NULL,
    "Path" text NOT NULL
);


ALTER TABLE public."File" OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16412)
-- Name: File_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."File" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."File_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 198 (class 1259 OID 16407)
-- Name: TitleFile; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TitleFile" (
    "Id" integer NOT NULL,
    "FileId" integer NOT NULL
);


ALTER TABLE public."TitleFile" OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16426)
-- Name: TitleFile_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."TitleFile" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."TitleFile_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 2828 (class 0 OID 16402)
-- Dependencies: 197
-- Data for Name: DescFile; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DescFile" ("Id", "FileId") FROM stdin;
1	1
2	2
3	3
\.


--
-- TOC entry 2827 (class 0 OID 16394)
-- Dependencies: 196
-- Data for Name: File; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."File" ("Id", "Path") FROM stdin;
1	c:\\TestFiles\\1.txt
2	c:\\TestFiles\\2.txt
3	c:\\TestFiles\\3.txt
4	c:\\TestFiles\\21.txt
5	c:\\TestFiles\\22.txt
6	c:\\TestFiles\\23.txt
11	c:\\TestFiles\\45.txt
12	c:\\Test\\jhg.txt
\.


--
-- TOC entry 2829 (class 0 OID 16407)
-- Dependencies: 198
-- Data for Name: TitleFile; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."TitleFile" ("Id", "FileId") FROM stdin;
1	4
2	5
3	6
\.


--
-- TOC entry 2839 (class 0 OID 0)
-- Dependencies: 200
-- Name: DescFile_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."DescFile_Id_seq"', 3, true);


--
-- TOC entry 2840 (class 0 OID 0)
-- Dependencies: 199
-- Name: File_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."File_Id_seq"', 12, true);


--
-- TOC entry 2841 (class 0 OID 0)
-- Dependencies: 201
-- Name: TitleFile_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."TitleFile_Id_seq"', 3, true);


--
-- TOC entry 2701 (class 2606 OID 16406)
-- Name: DescFile DescFile_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DescFile"
    ADD CONSTRAINT "DescFile_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2699 (class 2606 OID 16401)
-- Name: File File_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."File"
    ADD CONSTRAINT "File_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2703 (class 2606 OID 16411)
-- Name: TitleFile TitleFile_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TitleFile"
    ADD CONSTRAINT "TitleFile_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2704 (class 2606 OID 16414)
-- Name: DescFile fk_DeskFile_File; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DescFile"
    ADD CONSTRAINT "fk_DeskFile_File" FOREIGN KEY ("FileId") REFERENCES public."File"("Id") NOT VALID;


--
-- TOC entry 2705 (class 2606 OID 16419)
-- Name: TitleFile fk_TitleFile_File; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TitleFile"
    ADD CONSTRAINT "fk_TitleFile_File" FOREIGN KEY ("FileId") REFERENCES public."File"("Id") NOT VALID;


-- Completed on 2022-05-30 14:53:37

--
-- PostgreSQL database dump complete
--

