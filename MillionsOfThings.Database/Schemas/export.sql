--
-- PostgreSQL database dump
--

\restrict GI1iuyCydO6T0tApXF9gS2KbGLjyW79M8gdnxNIkwTClDqvnB2x3cpp5ojcqpJp

-- Dumped from database version 18.1 (Debian 18.1-1.pgdg13+2)
-- Dumped by pg_dump version 18.1 (Debian 18.1-1.pgdg13+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: category; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.category (
    category_id integer NOT NULL,
    user_id integer NOT NULL,
    name character varying(20) NOT NULL,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: category_category_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.category ALTER COLUMN category_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.category_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: list; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.list (
    list_id integer NOT NULL,
    user_id integer NOT NULL,
    name character varying(20) NOT NULL,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: list_list_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.list ALTER COLUMN list_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.list_list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: list_task_link; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.list_task_link (
    list_task_link_id integer NOT NULL,
    list_id integer NOT NULL,
    task_id integer NOT NULL,
    task_order integer NOT NULL,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: list_task_link_list_task_link_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.list_task_link ALTER COLUMN list_task_link_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.list_task_link_list_task_link_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: refresh_token; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.refresh_token (
    refresh_token_id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_id integer NOT NULL,
    token character varying(255) NOT NULL,
    created_by_ip character varying(39) NOT NULL,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: task; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.task (
    task_id integer NOT NULL,
    user_id integer NOT NULL,
    category_id integer,
    description character varying(255) NOT NULL,
    is_finished boolean DEFAULT false NOT NULL,
    finished_on timestamp(0) without time zone,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: task_task_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.task ALTER COLUMN task_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.task_task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: user; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."user" (
    user_id integer NOT NULL,
    is_allowed boolean NOT NULL,
    username character varying(20) NOT NULL,
    password character varying(100) NOT NULL,
    created_on timestamp(0) without time zone DEFAULT (now())::timestamp without time zone NOT NULL,
    modified_on timestamp(0) without time zone
);


--
-- Name: user2_user_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."user" ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.user2_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: category category_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_pkey PRIMARY KEY (category_id);


--
-- Name: category category_user_id_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_user_id_name_unique UNIQUE (user_id, name);


--
-- Name: list list_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list
    ADD CONSTRAINT list_pkey PRIMARY KEY (list_id);


--
-- Name: list_task_link list_task_link_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list_task_link
    ADD CONSTRAINT list_task_link_pkey PRIMARY KEY (list_task_link_id);


--
-- Name: refresh_token refresh_token_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT refresh_token_pkey PRIMARY KEY (refresh_token_id);


--
-- Name: task task_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_pkey PRIMARY KEY (task_id);


--
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (user_id);


--
-- Name: category category_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id) NOT VALID;


--
-- Name: list_task_link list_task_link_list_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list_task_link
    ADD CONSTRAINT list_task_link_list_id_fkey FOREIGN KEY (list_id) REFERENCES public.list(list_id);


--
-- Name: list_task_link list_task_link_task_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list_task_link
    ADD CONSTRAINT list_task_link_task_id_fkey FOREIGN KEY (task_id) REFERENCES public.task(task_id);


--
-- Name: list list_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list
    ADD CONSTRAINT list_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id) NOT VALID;


--
-- Name: refresh_token refresh_token_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT refresh_token_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: task task_category_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_category_id_fkey FOREIGN KEY (category_id) REFERENCES public.category(category_id);


--
-- Name: task task_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.task
    ADD CONSTRAINT task_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id) NOT VALID;


--
-- PostgreSQL database dump complete
--

\unrestrict GI1iuyCydO6T0tApXF9gS2KbGLjyW79M8gdnxNIkwTClDqvnB2x3cpp5ojcqpJp

