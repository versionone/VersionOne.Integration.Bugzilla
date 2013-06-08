#!/bin/bash
# From: http://happygiraffe.net/blog/2009/07/04/publishing-a-subdirectory-to-github-pages/

parent_sha=$(git show-ref -s refs/heads/gh-pages)
doc_sha=$(git ls-tree -d HEAD doc | awk '{print $3}')
#new_commit=$(echo "Auto-update docs." | git commit-tree $doc_sha -p $parent_sha)
new_commit=$(echo "Auto-update docs." | git commit-tree $doc_sha -p refs/heads/gh-pages)
git update-ref refs/heads/gh-pages $new_commit
