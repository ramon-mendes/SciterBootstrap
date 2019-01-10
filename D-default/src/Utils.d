wstring to_wstring(const wchar* pstr)
{
	int strlen(const wchar* pstr)
	{
		int c;
		while( *(pstr+c)!='\0' )
			c++;
		return c;
	}

	return pstr[0..strlen(pstr)].idup;
}